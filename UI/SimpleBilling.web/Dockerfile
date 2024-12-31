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

# Use custom Nginx configuration
COPY ./UI/SimpleBilling.web/nginx.conf /etc/nginx/nginx.conf 

# Expose port 80
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
