let cuentaId = null;

// ✅ LOGIN
function login() {
    let dpi = document.getElementById("dpiLogin").value;
    let password = document.getElementById("passLogin").value;

    fetch("https://localhost:7229/api/Cliente/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ dpi: dpi, password: password })
    })
        .then(res => res.json())
        .then(cliente => {

            return fetch("https://localhost:7229/api/Cuenta/crear?clienteId=" + cliente.id, {
                method: "POST"
            });
        })
        .then(res => res.json())
        .then(cuenta => {

            cuentaId = cuenta.id;

            document.getElementById("login").style.display = "none";
            document.getElementById("dashboard").style.display = "block";

            alert("Login correcto");
        })
        .catch(() => alert("Error login"));
}


// ✅ REGISTRO
function registrar() {
    let nombre = document.getElementById("nombre").value;
    let dpi = document.getElementById("dpiRegistro").value;
    let password = document.getElementById("passRegistro").value;

    fetch("https://localhost:7229/api/Cliente/registro", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            nombre: nombre,
            dpi: dpi,
            password: password
        })
    })
        .then(() => alert("Usuario registrado"))
        .catch(() => alert("Error"));
}


// ✅ LOGOUT
function logout() {
    location.reload();
}


// ✅ DEPOSITO
function depositar() {
    let monto = document.getElementById("depositoMonto").value;

    fetch("https://localhost:7229/api/Cuenta/deposito?cuentaId=" + cuentaId + "&monto=" + monto, {
        method: "POST"
    })
        .then(() => alert("Depósito realizado"));
}


// ✅ PAGO
function pagar() {
    let monto = document.getElementById("pagoMonto").value;

    fetch("https://localhost:7229/api/Banco/pagar?cuentaId=" + cuentaId + "&monto=" + monto + "&tipo=Universidad", {
        method: "POST"
    })
        .then(() => alert("Pago realizado"));
}


// ✅ TRANSFERENCIA
function transferir() {
    let destino = document.getElementById("cuentaDestino").value;
    let monto = document.getElementById("montoTransfer").value;

    fetch("https://localhost:7229/api/Banco/transferir?origenId=" + cuentaId + "&destinoId=" + destino + "&monto=" + monto, {
        method: "POST"
    })
        .then(() => alert("Transferencia realizada"));
}