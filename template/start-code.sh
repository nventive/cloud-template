#!/usr/bin/env bash

# Ensure we start at the right place
cd "$( dirname "${BASH_SOURCE[0]}" )"

# Eliminate environment variables introduced by the nix shell that interfere
# with Aspire debugging in VS Code.
export -n PYTHONPATH
export -n name
export -n shell

code .
