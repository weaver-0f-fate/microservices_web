FROM node

# Install OpenSSL and update Node.js/npm
RUN apt-get update && apt-get install -y openssl && \
    npm install -g npm@latest && \
    npm cache clean -f && \
    npm install -g n && \
    n stable

WORKDIR /app

# Installing dependencies
COPY package*.json /app/
RUN npm install

# Copying source files
COPY . .

EXPOSE 3000

# Running the app
CMD ["npm", "run", "docker_start"]