document.addEventListener("DOMContentLoaded", () => {

    const nombre =
        document.getElementById("nombreProducto");

    const precio =
        document.getElementById("precioProducto");

    const stock =
        document.getElementById("stockProducto");

    // ======================
    // NOMBRE
    // ======================

    nombre.addEventListener("input", () => {

        const error =
            document.getElementById("errorNombre");

        if (nombre.value.trim().length < 3) {

            error.textContent =
                "Debe tener al menos 3 caracteres";

            nombre.classList.add("is-invalid");
        }
        else {

            error.textContent = "";

            nombre.classList.remove("is-invalid");
            nombre.classList.add("is-valid");
        }

    });

    // ======================
    // PRECIO
    // ======================

    precio.addEventListener("input", () => {

        const error =
            document.getElementById("errorPrecio");

        if (parseFloat(precio.value) <= 0) {

            error.textContent =
                "El precio debe ser mayor que 0";

            precio.classList.add("is-invalid");
        }
        else {

            error.textContent = "";

            precio.classList.remove("is-invalid");
            precio.classList.add("is-valid");
        }

    });

    // ======================
    // STOCK
    // ======================

    stock.addEventListener("input", () => {

        const error =
            document.getElementById("errorStock");

        if (parseInt(stock.value) < 0) {

            error.textContent =
                "El stock no puede ser negativo";

            stock.classList.add("is-invalid");
        }
        else {

            error.textContent = "";

            stock.classList.remove("is-invalid");
            stock.classList.add("is-valid");
        }

    });

    // ======================
    // LOCAL STORAGE
    // ======================

    nombre.addEventListener("change", () => {

        localStorage.setItem(
            "ultimoProducto",
            nombre.value
        );

    });

});