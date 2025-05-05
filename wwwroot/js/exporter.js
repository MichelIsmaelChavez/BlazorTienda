window.exportarTablaAPDF = async function (idTabla) {
    const tabla = document.getElementById(idTabla);
    if (!tabla) {
        console.error("Tabla no encontrada: " + idTabla);
        return;
    }

    const canvas = await html2canvas(tabla, { scale: 2 });
    const imgData = canvas.toDataURL('image/png');

    const { jsPDF } = window.jspdf;
    const pdf = new jsPDF('l', 'pt', 'a4'); // landscape, points, A4
    const imgProps = pdf.getImageProperties(imgData);
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;

    pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
    pdf.save('ControlPagosClientes.pdf');
}
