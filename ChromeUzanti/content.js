
chrome.runtime.sendMessage({ action: "testMessage" }, function(response) {
  if (chrome.runtime.lastError) {
    console.error("Hata:", chrome.runtime.lastError.message);
  } else {
    console.log("Mesaj başarıyla gönderildi", response);
  }
});


chrome.runtime.onMessage.addListener((message) => {
  if (message.action === "autoLogin") {
    console.log("AutoLogin mesajı alındı:", message);

    
    autoLogin(message.username, message.password);
  }
});


window.addEventListener("message", (event) => {
  if (event.data && event.data.type === "autoLoginRequest") {
    
    chrome.runtime.sendMessage({
      action: event.data.action,
      url: event.data.url,
      username: event.data.username,
      password: event.data.password
    });
  }
});


function autoLogin(username, password) {
  const { usernameField, passwordField } = findLoginFields();

  if (usernameField && passwordField) {
    usernameField.value = username;
    passwordField.value = password;

    const form = usernameField.closest('form');
    if (form) {
      form.submit();
    } else {
      const submitButton = document.querySelector("button[type='submit'], input[type='submit']");
      if (submitButton) {
        submitButton.click();
      }
    }
  } else {
    console.error('Giriş alanları bulunamadı.');
  }
}


function findLoginFields() {
  const inputs = document.querySelectorAll('input');

  const usernameField = Array.from(inputs).find(input => {
    const type = input.getAttribute('type');
    const name = input.getAttribute('name');
    const id = input.getAttribute('id');

    return (
      (type === 'text' || type === 'email') && 
      (name && (name.toLowerCase().includes('user') || name.toLowerCase().includes('email') || name.toLowerCase().includes('login'))) ||
      (id && (id.toLowerCase().includes('user') || id.toLowerCase().includes('email') || id.toLowerCase().includes('login')))
    );
  });

  const passwordField = Array.from(inputs).find(input => {
    const type = input.getAttribute('type');
    const name = input.getAttribute('name');
    const id = input.getAttribute('id');

    return (
      type === 'password' && 
      (name && name.toLowerCase().includes('password')) ||
      (id && id.toLowerCase().includes('password'))
    );
  });

  return { usernameField, passwordField };
}
