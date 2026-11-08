document.querySelectorAll("[data-vietnam-location]").forEach((location, locationIndex) => {
  const province = location.querySelector("[data-province]");
  const ward = location.querySelector("[data-ward]");
  const wardOptions = [...ward.options];
  const comboboxes = [];

  const normalize = (value) => value.normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/đ/g, "d").replace(/Đ/g, "D").toLocaleLowerCase("vi");

  const enhance = (select, availableOptions) => {
    const wrapper = document.createElement("div");
    const input = document.createElement("input");
    const list = document.createElement("div");
    const inputId = `${select.id}-combobox-${locationIndex}`;
    const listId = `${inputId}-listbox`;
    let visible = [];
    let activeIndex = -1;

    wrapper.className = "location-combobox";
    input.id = inputId;
    input.type = "text";
    input.autocomplete = "off";
    input.placeholder = select.options[0]?.text ?? "";
    input.required = select.required;
    input.setAttribute("role", "combobox");
    input.setAttribute("aria-autocomplete", "list");
    input.setAttribute("aria-expanded", "false");
    input.setAttribute("aria-controls", listId);
    list.id = listId;
    list.className = "location-combobox-list";
    list.setAttribute("role", "listbox");
    list.hidden = true;

    select.insertAdjacentElement("afterend", wrapper);
    wrapper.append(input, list);
    select.classList.add("location-native-select");
    select.required = false;
    location.querySelector(`label[for="${select.id}"]`)?.setAttribute("for", inputId);

    const close = () => {
      list.hidden = true;
      input.setAttribute("aria-expanded", "false");
      input.removeAttribute("aria-activedescendant");
      activeIndex = -1;
    };

    const setActive = (index) => {
      if (!visible.length) return;
      activeIndex = (index + visible.length) % visible.length;
      visible.forEach((option, i) => option.classList.toggle("is-active", i === activeIndex));
      input.setAttribute("aria-activedescendant", visible[activeIndex].id);
      visible[activeIndex].scrollIntoView({ block: "nearest" });
    };

    const choose = (option) => {
      select.value = option.dataset.value;
      input.value = option.textContent;
      select.dispatchEvent(new Event("change", { bubbles: true }));
      close();
      input.focus();
    };

    const render = () => {
      const query = normalize(input.value);
      list.replaceChildren();
      visible = availableOptions().filter((option) => option.value && normalize(option.text).includes(query)).map((option, index) => {
        const item = document.createElement("div");
        item.id = `${listId}-option-${index}`;
        item.className = "location-combobox-option";
        item.dataset.value = option.value;
        item.textContent = option.text;
        item.setAttribute("role", "option");
        item.setAttribute("aria-selected", String(option.selected));
        item.addEventListener("mousedown", (event) => event.preventDefault());
        item.addEventListener("click", () => choose(item));
        list.append(item);
        return item;
      });
      if (!visible.length) {
        const empty = document.createElement("div");
        empty.className = "location-combobox-empty";
        empty.textContent = "Không tìm thấy";
        list.append(empty);
      }
      list.hidden = false;
      input.setAttribute("aria-expanded", "true");
      activeIndex = -1;
    };

    const sync = () => {
      input.value = select.selectedOptions[0]?.value ? select.selectedOptions[0].text : "";
      input.disabled = select.disabled;
      input.placeholder = select.disabled ? "Chọn tỉnh/thành trước" : select.options[0]?.text ?? "";
    };

    input.addEventListener("focus", render);
    input.addEventListener("input", () => {
      select.value = "";
      render();
    });
    input.addEventListener("keydown", (event) => {
      if (event.key === "Escape") close();
      else if (event.key === "ArrowDown" || event.key === "ArrowUp") {
        event.preventDefault();
        if (list.hidden) render();
        setActive(activeIndex + (event.key === "ArrowDown" ? 1 : -1));
      } else if (event.key === "Enter" && activeIndex >= 0) {
        event.preventDefault();
        choose(visible[activeIndex]);
      }
    });

    const combobox = { wrapper, close, sync };
    comboboxes.push(combobox);
    sync();
    return combobox;
  };

  const provinceCombobox = enhance(province, () => [...province.options]);
  const wardCombobox = enhance(ward, () => wardOptions.filter((option) => !option.value || option.dataset.provinceCode === province.value));

  const filterWards = () => {
    const selected = ward.value;
    wardOptions.forEach((option) => {
      option.hidden = !!option.value && option.dataset.provinceCode !== province.value;
    });
    if (!wardOptions.some((option) => option.value === selected && !option.hidden)) ward.value = "";
    ward.disabled = !province.value;
    provinceCombobox.sync();
    wardCombobox.sync();
  };

  province.addEventListener("change", filterWards);
  ward.addEventListener("change", wardCombobox.sync);
  document.addEventListener("click", (event) => {
    comboboxes.forEach((combobox) => {
      if (!combobox.wrapper.contains(event.target)) combobox.close();
    });
  });
  filterWards();
});
