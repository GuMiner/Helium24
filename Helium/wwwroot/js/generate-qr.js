window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    const qrCode = new QRCodeStyling({
        width: 300,
        height: 300,
        type: "svg",
        data: uri,
        image: "../../../favicon.png",
        dotsOptions: {
            color: "#256119",
            type: "classy-rounded"
        },
        cornersSquareOptions: {
            type: "",
            color: "#000000"
        },
        backgroundOptions: {
            color: "#ffffff",
        },
        imageOptions: {
            margin: 20
        }
    });

    qrCode.append(document.getElementById("qrCode"));
    qrCode.download({ name: "qr", extension: "svg" });
});