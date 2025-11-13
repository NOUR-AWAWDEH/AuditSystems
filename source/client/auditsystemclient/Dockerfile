# Build stage
FROM node:lts-alpine AS build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm run build

# Production stage
FROM node:lts-alpine AS production-stage
WORKDIR /app
COPY --from=build-stage /app/dist ./dist
COPY package*.json ./
COPY vite.config.js ./
# Install including dev dependencies so vite preview is available at runtime
RUN npm install
EXPOSE 10000
CMD ["npm", "run", "start"]