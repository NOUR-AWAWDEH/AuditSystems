# Build stage
FROM node:24.22.1-alpine@sha256:06d7adf3e0ef61a13f1d28682726366f97a15aa1888c934d3ecec86c33918b53 AS build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm run build

# Production stage
FROM node:24.22.1-alpine@sha256:06d7adf3e0ef61a13f1d28682726366f97a15aa1888c934d3ecec86c33918b53 AS production-stage
WORKDIR /app
COPY --from=build-stage /app/dist ./dist
COPY package*.json ./
COPY vite.config.js ./
# Install including dev dependencies so vite preview is available at runtime
RUN npm install
EXPOSE 10000
CMD ["npm", "run", "start"]