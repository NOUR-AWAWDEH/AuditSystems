# Build stage
FROM node:lts-alpine as build-stage
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
ARG VITE_API_URL
ENV VITE_API_URL=$VITE_API_URL
RUN npm run build

# Production stage
FROM node:lts-alpine as production-stage
WORKDIR /app
COPY --from=build-stage /app/dist ./dist
COPY package*.json ./
RUN npm install --only=production
EXPOSE 10000
CMD ["npm", "run", "start"]