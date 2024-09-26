console.log("background.js çalışıyor");

chrome.runtime.onMessage.addListener((message, sender, sendResponse) => {
  if (message.action === "autoLogin") {
    console.log("AutoLogin isteği alındı:", message);

    
    chrome.tabs.create({ url: message.url }, (tab) => {
      chrome.scripting.executeScript({
        target: { tabId: tab.id },
        func: autoLoginFunction,
        args: [message.url, message.username, message.password]
      });
    });

    sendResponse({ success: true, message: "İşlem başarıyla tamamlandı." });
  } else {
    
    
    sendResponse({ success: false, message: `Bilinmeyen işlem: ${message.action}` });
  }
});


function autoLoginFunction(url, username, password) {
  console.log("AutoLogin işlevi çalışıyor:", username, password);

  
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

  
  const { usernameField, passwordField } = findLoginFields();
  if (usernameField && passwordField) {
    console.log("Giriş alanları bulundu:");
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
    console.error("Giriş alanları bulunamadı.");
  }
}
