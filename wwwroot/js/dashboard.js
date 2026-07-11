document.addEventListener("DOMContentLoaded", () => {

    const contadores =
        document.querySelectorAll(".contador");

    contadores.forEach(contador => {

        const objetivo =
            parseInt(contador.innerText);

        let actual = 0;

        const intervalo = setInterval(() => {

            actual++;

            contador.innerText = actual;

            if (actual >= objetivo) {

                clearInterval(intervalo);

            }

        }, 20);

    });

});