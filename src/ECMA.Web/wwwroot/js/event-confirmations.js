document.addEventListener("submit", (event) => {
  const form = event.target;
  if (!(form instanceof HTMLFormElement)) return;

  const message = form.dataset.confirm;
  if (message && !window.confirm(message)) {
    event.preventDefault();
    return;
  }

  const location = form.querySelector("[data-vietnam-location][data-original-location]");
  if (!location) return;

  const street = location.querySelector("[data-street-address]").value.trim();
  const ward = location.querySelector("[data-ward]").selectedOptions[0]?.text ?? "";
  const province = location.querySelector("[data-province]").selectedOptions[0]?.text ?? "";
  const composed = `${street}, ${ward}, ${province}`.normalize("NFC");
  if (composed !== location.dataset.originalLocation.trim().normalize("NFC") && !window.confirm("Địa điểm mới sẽ hiển thị cho người tham dự. Hệ thống không gửi thông báo về thay đổi này")) {
    event.preventDefault();
  }
});
