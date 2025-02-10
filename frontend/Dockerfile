FROM node:23-alpine
WORKDIR /app/riihisoft

# install node packages
COPY ./package.json .
COPY ./package-lock.json .
RUN npm ci

# copy the rest of the files
COPY . .

# change the frontend proxy url
RUN sed -i 's/localhost:5103/backend:80/' vite.config.ts

CMD ["npm", "run", "docker"]
