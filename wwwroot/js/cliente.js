let mapa;
let marcador;

window.initMapa = (latInicial, lngInicial, dotNetRef) => {
    // Determina el ID del mapa según cuál exista en el DOM
    let mapaId = document.getElementById('mapaPerfil') ? 'mapaPerfil' :
        document.getElementById('mapa') ? 'mapa' : null;

    if (!mapaId) {
        console.error("❌ No se encontró un contenedor de mapa válido (#mapaPerfil o #mapa)");
        return;
    }

    // Si ya había un mapa anterior, elimínalo completamente
    if (mapa) {
        mapa.remove();
        mapa = null;
    }

    mapa = L.map(mapaId).setView([latInicial, lngInicial], 15);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    }).addTo(mapa);

    marcador = L.marker([latInicial, lngInicial], { draggable: true }).addTo(mapa);

    marcador.on("dragend", function (e) {
        const pos = marcador.getLatLng();
        dotNetRef.invokeMethodAsync('EstablecerUbicacion', pos.lat, pos.lng);
    });

    mapa.on("click", function (e) {
        const lat = e.latlng.lat;
        const lng = e.latlng.lng;
        marcador.setLatLng([lat, lng]);
        dotNetRef.invokeMethodAsync('EstablecerUbicacion', lat, lng);
    });

    // Opcional: usar geolocalización si está disponible
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition((pos) => {
            const lat = pos.coords.latitude;
            const lng = pos.coords.longitude;
            mapa.setView([lat, lng], 15);
            marcador.setLatLng([lat, lng]);
            dotNetRef.invokeMethodAsync('EstablecerUbicacion', lat, lng);
        });
    }
};
