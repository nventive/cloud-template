FROM node:20 as build

WORKDIR /app

COPY package.json package.json
COPY yarn.lock yarn.lock

RUN yarn install --immutable

COPY . .

RUN yarn build

FROM nginx:alpine

COPY --from=build /app/default.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

# Expose the default nginx port
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
