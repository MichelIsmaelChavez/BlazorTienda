// Funciones de ayuda para Blazor
const blazorHelpers = {
    showToast: function (type, title, message) {
        // Crear contenedor si no existe
        let container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.style.position = 'fixed';
            container.style.bottom = '20px';
            container.style.right = '20px';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }

        // Crear toast
        const toast = document.createElement('div');
        toast.className = `toast show align-items-center text-white bg-${type} border-0 mb-2`;
        toast.style.minWidth = '300px';
        toast.style.maxWidth = '350px';
        toast.style.opacity = '1';
        toast.style.transition = 'opacity 0.5s ease';

        toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                <strong>${title}</strong><br>${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" onclick="this.parentElement.parentElement.style.opacity='0'; setTimeout(() => this.parentElement.parentElement.remove(), 500)"></button>
        </div>
        `;

        container.appendChild(toast);

        // Auto-eliminar después de 5 segundos
        setTimeout(() => {
            toast.style.opacity = '0';
            setTimeout(() => toast.remove(), 500);
        }, 5000);
    },

    confirmarDelivery: function (message) {
        return new Promise((resolve) => {
            const confirmed = window.confirm(message);
            resolve(confirmed);
        });
    }
};

// Hacer las funciones disponibles globalmente
window.blazorHelpers = blazorHelpers;
