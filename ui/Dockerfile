# UI Runtime Dockerfile
# Expects pre-built .next folder and node_modules
FROM node:20-alpine

WORKDIR /app

# Install pnpm
RUN corepack enable && corepack prepare pnpm@latest --activate

# Copy pre-built artifacts
COPY ui/.next ./.next
COPY ui/node_modules ./node_modules
COPY ui/package.json ./
COPY ui/public ./public

EXPOSE 3000
ENV NODE_ENV=production
ENV PORT=3000

CMD ["pnpm", "start"]
