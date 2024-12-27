# Stage 1: Build the Angular application
FROM node:18 AS build-stage
WORKDIR /app

# Install dependencies
COPY package*.json ./  
RUN npm install

# Copy the Angular project and build
COPY . . 
RUN npm run build -- --configuration production 

# Stage 2: Serve the application using Nginx
FROM nginx:alpine AS production-stage

# Copy the built files to Nginx
COPY --from=build-stage /app/dist/simple-billing.web/browser /usr/share/nginx/html

# Use custom Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf 

# Expose port 80
EXPOSE 80  

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
