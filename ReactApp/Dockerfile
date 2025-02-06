# Используем официальный образ Node.js для сборки
FROM node:16 AS build

# Устанавливаем рабочую директорию
WORKDIR /app

# Копируем package.json и package-lock.json
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем исходный код
COPY . .

# Собираем приложение
RUN npm run build

# Используем легковесный образ Nginx для запуска приложения
FROM nginx:alpine

# Копируем собранные файлы из предыдущего этапа
COPY --from=build /app/build /usr/share/nginx/html

# Копируем конфигурацию Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Открываем порт 80
EXPOSE 80

# Запускаем Nginx
CMD ["nginx", "-g", "daemon off;"]