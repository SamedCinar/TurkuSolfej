// API Endpoint'leri
const API_BASE_URL = 'http://localhost:5000/api';

// Sayfa yüklendiğinde çalışacak fonksiyonlar
document.addEventListener('DOMContentLoaded', () => {
    loadPopularTurkuler();
    setupSearchHandler();
});

// Popüler türküleri yükleme
async function loadPopularTurkuler() {
    try {
        const response = await fetch(`${API_BASE_URL}/turkuler/populer`);
        const turkuler = await response.json();
        
        const turkulerGrid = document.querySelector('.turkuler-grid');
        turkuler.forEach(turku => {
            const turkuCard = createTurkuCard(turku);
            turkulerGrid.appendChild(turkuCard);
        });
    } catch (error) {
        console.error('Türküler yüklenirken hata oluştu:', error);
    }
}

// Türkü kartı oluşturma
function createTurkuCard(turku) {
    const card = document.createElement('div');
    card.className = 'turku-card';
    card.innerHTML = `
        <img src="${turku.imageUrl}" alt="${turku.title}">
        <h3>${turku.title}</h3>
        <p>${turku.artist}</p>
        <button onclick="showNota('${turku.id}')">Notaları Göster</button>
    `;
    return card;
}

// Arama işleyicisi
function setupSearchHandler() {
    const searchInput = document.querySelector('.nota-arama input');
    const searchButton = document.querySelector('.nota-arama button');

    searchButton.addEventListener('click', () => {
        const searchTerm = searchInput.value.trim();
        if (searchTerm) {
            searchTurkuler(searchTerm);
        }
    });

    searchInput.addEventListener('keypress', (e) => {
        if (e.key === 'Enter') {
            const searchTerm = searchInput.value.trim();
            if (searchTerm) {
                searchTurkuler(searchTerm);
            }
        }
    });
}

// Türkü arama
async function searchTurkuler(searchTerm) {
    try {
        const response = await fetch(`${API_BASE_URL}/turkuler/search?q=${encodeURIComponent(searchTerm)}`);
        const results = await response.json();
        displaySearchResults(results);
    } catch (error) {
        console.error('Arama yapılırken hata oluştu:', error);
    }
}

// Arama sonuçlarını gösterme
function displaySearchResults(results) {
    const turkulerGrid = document.querySelector('.turkuler-grid');
    turkulerGrid.innerHTML = ''; // Mevcut sonuçları temizle

    if (results.length === 0) {
        turkulerGrid.innerHTML = '<p>Sonuç bulunamadı.</p>';
        return;
    }

    results.forEach(turku => {
        const turkuCard = createTurkuCard(turku);
        turkulerGrid.appendChild(turkuCard);
    });
}

// Nota gösterme
async function showNota(turkuId) {
    try {
        const response = await fetch(`${API_BASE_URL}/turkuler/${turkuId}/nota`);
        const nota = await response.json();
        displayNota(nota);
    } catch (error) {
        console.error('Nota yüklenirken hata oluştu:', error);
    }
}

// Nota gösterme modalı
function displayNota(nota) {
    // Modal oluşturma ve gösterme işlemleri burada yapılacak
    console.log('Nota gösteriliyor:', nota);
}

// Kullanıcı girişi kontrolü
function checkAuth() {
    const token = localStorage.getItem('authToken');
    return !!token;
}

// Sayfa yönlendirme
function navigateTo(page) {
    window.location.href = `/${page}`;
} 