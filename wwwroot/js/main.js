// Основной файл, который будет инициализировать приложение и связывать модули
import { initUI, renderTransactions } from './ui.js';

document.addEventListener("DOMContentLoaded", () => {
    initUI();
    renderTransactions();
});
