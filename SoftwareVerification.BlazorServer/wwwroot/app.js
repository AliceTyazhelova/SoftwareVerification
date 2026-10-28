window.showToast = function (message) {
    const container = document.getElementById('toast-container');
    if (!container) {
        console.warn('Контейнер для Toast не найден!');
        return;
    }

    // Создаем элемент Toast
    const toastEl = document.createElement('div');
    toastEl.className = 'toast align-items-center text-bg-success border-0';
    toastEl.setAttribute('role', 'alert');
    toastEl.setAttribute('aria-live', 'assertive');
    toastEl.setAttribute('aria-atomic', 'true');

    toastEl.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;

    container.appendChild(toastEl);

    // Показываем через Bootstrap
    const toast = new bootstrap.Toast(toastEl, { delay: 5000 });
    toast.show();

    // Удаляем из DOM после скрытия
    toastEl.addEventListener('hidden.bs.toast', () => {
        toastEl.remove();
    });
};

// Функция для "встряхивания" колокольчика
window.updateBell = function () {
    // Находим колокольчик на странице по ID
    const bell = document.getElementById('notification-bell');
    if (bell) {
        // Добавляем CSS-анимацию (например, покачивание)
        bell.classList.add('bell-shake');

        // Убираем класс через 500мс, чтобы анимация могла повториться
        setTimeout(() => {
            bell.classList.remove('bell-shake');
        }, 500);
    }
};