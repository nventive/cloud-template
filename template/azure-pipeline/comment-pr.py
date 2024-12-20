import os
import requests
import openai
import difflib

API_VERSION = "7.1"

# Utility Functions


def get_env_variable(var_name, required=True):
    value = os.getenv(var_name)
    if required and not value:
        raise EnvironmentError(f"Environment variable '{
                               var_name}' is missing.")
    return value.strip("/")


def make_api_request(url, headers, method="GET", data=None):
    response = requests.request(method, url, headers=headers, json=data)
    if response.status_code != 200:
        raise Exception(f"API call failed: {
                        response.status_code}, Response: {response.text}")
    return response.json()


def compare_file_contents(file_path, old_content, new_content):
    diff = difflib.unified_diff(
        old_content.splitlines(),
        new_content.splitlines(),
        fromfile=f"{file_path} (old)",
        tofile=f"{file_path} (new)"
    )
    return "\n".join(diff)


def calculate_cost(prompt_tokens, completion_tokens):
    input_rate = 0.15 / 1_000_000  # $0.15 per 1M input tokens
    output_rate = 0.6 / 1_000_000  # $0.60 per 1M output tokens
    return (prompt_tokens * input_rate) + (completion_tokens * output_rate)


def main():
    # Load Environment Variables
    azure_token = get_env_variable("SYSTEM_ACCESSTOKEN")
    openai_api_key = get_env_variable("OPENAI_API_KEY")
    organization = get_env_variable("SYSTEM_COLLECTIONURI")
    project = get_env_variable("SYSTEM_TEAMPROJECT")
    repo_id = get_env_variable("BUILD_REPOSITORY_ID")
    pr_id = get_env_variable("SYSTEM_PULLREQUEST_PULLREQUESTID")

    # Configure API and Headers
    openai.api_key = openai_api_key
    headers = {"Authorization": f"Bearer {azure_token}"}
    base_url = f"{organization}/{project}/_apis/git/repositories/{repo_id}"

    # Fetch Pull Request Details
    pr_url = f"{base_url}/pullRequests/{pr_id}?api-version=7.0"
    pr_details = make_api_request(pr_url, headers)
    title = pr_details["title"]
    description = pr_details.get("description", "")
    target_branch = pr_details["targetRefName"].split("/")[-1]

    # Fetch Iterations and Changed Files
    iterations_url = f"{
        base_url}/pullRequests/{pr_id}/iterations?api-version={API_VERSION}"
    iterations = make_api_request(iterations_url, headers)
    changed_files = set()

    if "value" in iterations:
        for iteration in iterations["value"]:
            iteration_id = iteration["id"]
            commits_url = f"{base_url}/pullRequests/{pr_id}/iterations/{
                iteration_id}/commits?api-version={API_VERSION}"
            commits = make_api_request(commits_url, headers)["value"]

            for commit in commits:
                commit_id = commit["commitId"]
                commit_url = f"{
                    base_url}/commits/{commit_id}?api-version={API_VERSION}"
                commit_data = make_api_request(commit_url, headers)
                changes_url = commit_data["_links"]["changes"]["href"]
                changes_data = make_api_request(changes_url, headers)
                if "changes" in changes_data:
                    for change in changes_data["changes"]:
                        if "item" in change and "path" in change["item"]:
                            path = change["item"]["path"]
                            if "." in path.split("/")[-1]:
                                changed_files.add(path)

    # Analyze File Changes
    messages = [
        {"role": "system", "content": "You are a helpful and knowledgeable code reviewer with expertise in clean code practices."},
        {"role": "user", "content": f"Here's a PR titled '{title}' with the following description: {
            description}. Please review the implementation carefully."},
        {"role": "user", "content": f"The following files were changed: {changed_files}."}
    ]

    for file_path in changed_files:
        print(f"Processing file: {file_path}")
        try:
            # Fetch New Version
            new_version_url = f"{base_url}/items/{file_path}?versionType=Commit&version={
                commits[-1]['commitId']}&api-version={API_VERSION}"
            new_version_content = requests.get(
                new_version_url, headers=headers).text

            # Fetch Previous Version
            prev_version_url = f"{base_url}/items?path={
                file_path}&versionDescriptor[versionType]=branch&versionDescriptor[version]={target_branch}&api-version={API_VERSION}"
            prev_version_content = requests.get(
                prev_version_url, headers=headers).text

            # Generate Diff
            diff = compare_file_contents(
                file_path, prev_version_content, new_version_content)
            messages.append(
                {"role": "user", "content": f"Changes for file: {file_path}\n{diff}"})

        except Exception as e:
            print(f"Failed to process file {file_path}: {e}")
            continue

    # OpenAI Completion
    response = openai.ChatCompletion.create(
        model="gpt-4o-mini",
        messages=messages,
        max_tokens=5000
    )
    comment = response["choices"][0]["message"]["content"]

    # Post Comment
    comment_cost = calculate_cost(
        response["usage"]["prompt_tokens"],
        response["usage"]["completion_tokens"]
    )
    comment_url = f"{base_url}/pullRequests/{pr_id}/threads?api-version=7.0"
    comment_body = {
        "comments": [{"content": f"{comment}\n\nTotal cost: ${comment_cost:.2f}"}],
        "status": "active"
    }
    requests.post(comment_url, headers=headers, json=comment_body)


if __name__ == "__main__":
    main()
