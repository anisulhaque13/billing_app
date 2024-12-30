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

# Use a custom Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Dynamically set the PORT for Nginx
ENV PORT=8080

# Replace default Nginx configuration to listen on the dynamically assigned PORT
RUN envsubst '${PORT}' < /etc/nginx/nginx.conf > /etc/nginx/nginx.conf.tmp && mv /etc/nginx/nginx.conf.tmp /etc/nginx/nginx.conf

# Expose the port (matches the Cloud Run environment)
EXPOSE 8080

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
