// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Mobile menu

document.addEventListener('DOMContentLoaded', function () {
  const btn = document.getElementById('mobile-menu-btn');
  const menu = document.getElementById('mobile-menu');

  if (btn && menu) {
    btn.addEventListener('click', function () {
      console.log('clicked');
      menu.classList.toggle('hidden');
    });
  }
});

// Qoutes

const quoteBtn = document.getElementById('get-quote-btn');

if (quoteBtn) {
  quoteBtn.addEventListener('click', async function (e) {
    e.preventDefault();

    try {
      const response = await fetch('/Home/GetQuote');
      const quote = await response.json();
      // const quote = data[0];

      document.getElementById('quote-text').textContent = `"${quote.content}"`;
      document.getElementById('quote-author').textContent = `"${quote.author}"`;
      document.getElementById('quote-container').classList.remove('hidden');
    } catch (error) {
      console.error('Failed to fetch quote: ', error);
    }
  });
}

// FaqItems
// TODO: Add IsActive
document.querySelectorAll('.faq-btn').forEach(function (btn) {
  btn.addEventListener('click', function () {
    const answer = btn.nextElementSibling;
    const icon = btn.querySelector('.fa-chevron-down');

    answer.classList.toggle('hidden');
    icon.classList.toggle('rotate-180');
  });
});

setTimeout(function () {
  document.querySelectorAll('.toast-message').forEach(function (el) {
    el.remove();
  });
}, 3000);
