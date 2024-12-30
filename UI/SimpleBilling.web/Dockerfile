# Stage 1: Build the Angular application
FROM node:18 AS build-stage
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire Angular project
COPY . .

# Build the Angular application in production mode
RUN npm run build -- --configuration production

# Stage 2: Serve the application using Nginx
FROM nginx:alpine AS production-stage

# Copy the built Angular application to Nginx's default HTML directory
COPY --from=build-stage /app/dist/simple-billing.web/browser /usr/share/nginx/html

# Copy custom Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf.template

# Install envsubst for dynamic environment variable substitution
RUN apk add --no-cache gettext

# Define the BACKEND_URL as an environment variable
ENV BACKEND_URL=https://simplebilling-backend-xxxx.a.run.app  

# Use envsubst to replace BACKEND_URL in the Nginx configuration
RUN envsubst '${BACKEND_URL}' < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf

# Expose the dynamically assigned port
EXPOSE 8080

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
