// ✅ Solo se declara una vez globalmente
let mapa;
let marcador;

// 📍 Mapa editable con click y marcador
window.initMapa = (lat, lng, dotnetHelper) => {
    if (mapa && mapa._container) {
        mapa.remove();
    }

    const contenedor = document.getElementById("mapa");
    if (!contenedor) return;

    mapa = L.map("mapa").setView([lat, lng], 14);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png").addTo(mapa);

    marcador = L.marker([lat, lng], { draggable: true }).addTo(mapa);

    dotnetHelper.invokeMethodAsync("EstablecerUbicacion", lat, lng);

    mapa.on("click", function (e) {
        const { lat, lng } = e.latlng;
        marcador.setLatLng([lat, lng]);
        dotnetHelper.invokeMethodAsync("EstablecerUbicacion", lat, lng);

        const input = document.getElementById("direccionInput");
        if (input) input.value = `${lat}, ${lng}`;

        fetch(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lng}`)
            .then(response => response.json())
            .then(data => {
                const direccion = data.display_name || `${lat}, ${lng}`;
                if (input) input.value = direccion;
                window.direccionTexto = direccion;
            })
            .catch(err => {
                console.error("❌ Error al obtener la dirección:", err);
            });
    });
};

// 📍 Mapa perfil con click
window.initMapaPerfil = (lat, lng, dotnetHelper) => {
    if (window._mapaPerfil && window._mapaPerfil._container) {
        window._mapaPerfil.remove();
    }

    const contenedor = document.getElementById("mapaPerfil");
    if (!contenedor) return;

    window._mapaPerfil = L.map('mapaPerfil').setView([lat, lng], 14);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png').addTo(window._mapaPerfil);

    let marker = L.marker([lat, lng]).addTo(window._mapaPerfil);

    window._mapaPerfil.on('click', function (e) {
        const { lat, lng } = e.latlng;
        marker.setLatLng([lat, lng]);
        dotnetHelper.invokeMethodAsync("EstablecerUbicacion", lat, lng);
    });
};

// 🚚 Muestra ruta delivery → cliente
window.mostrarMapaEntregaRuta = function (mapId, latDelivery, lngDelivery, latCliente, lngCliente) {
    const mapDiv = document.getElementById(mapId);
    mapDiv.innerHTML = "";

    const map = L.map(mapId).setView([latCliente, lngCliente], 13);
    L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
        attribution: "Leaflet"
    }).addTo(map);

    const markerDelivery = L.marker([latDelivery, lngDelivery]).addTo(map).bindPopup("Delivery").openPopup();
    const markerCliente = L.marker([latCliente, lngCliente]).addTo(map).bindPopup("Cliente");

    const ruta = L.polyline([
        [latDelivery, lngDelivery],
        [latCliente, lngCliente]
    ], { color: "blue", weight: 4, opacity: 0.7 }).addTo(map);

    map.fitBounds(ruta.getBounds(), { padding: [30, 30] });
};
