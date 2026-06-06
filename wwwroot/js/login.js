document.addEventListener("DOMContentLoaded", () => {

    const password = document.getElementById("password");

    const boton = document.getElementById("btnMostrarPassword");

    const email = document.getElementById("email");

    if (boton && password) {

        boton.addEventListener("click", () => {

            password.type =
                password.type === "password"
                    ? "text"
                    : "password";

        });

    }

    if (email) {

        const correoGuardado = localStorage.getItem("ultimoCorreo");

        if (correoGuardado) {
            email.value = correoGuardado;
        }

        email.addEventListener("change", () => {

            localStorage.setItem("ultimoCorreo", email.value);

        });

    }

});
