// Game state
let canvas;
let ctx;
let lastTime = 0;
let deltaTime = 0;

// Game objects
let spaceship;
let bullets = [];
let enemyBullets = []; // Düşman mermileri için yeni array
let enemies = [];
let score = 0;
let gameOver = false;

// Input state
const keys = {
    ArrowLeft: false,
    ArrowRight: false,
    ArrowUp: false,
    ArrowDown: false,
    Space: false
};

// Game constants
const ENEMY_SPAWN_INTERVAL = 2000; // 2 saniyede bir düşman
let lastEnemySpawn = 0;

// Initialize game
function init() {
    canvas = document.getElementById('gameCanvas');
    ctx = canvas.getContext('2d');
    
    // Create spaceship
    spaceship = {
        x: canvas.width / 2 - 25,
        y: canvas.height - 100,
        width: 50,
        height: 50,
        health: 100,
        speed: 300,
        lastShot: 0,
        fireRate: 250 // ms between shots
    };

    // Event listeners
    document.addEventListener('keydown', handleKeyDown);
    document.addEventListener('keyup', handleKeyUp);

    // Start game loop
    requestAnimationFrame(gameLoop);
}

// Input handlers
function handleKeyDown(e) {
    if (e.code in keys) {
        keys[e.code] = true;
        if (e.code === 'Space') {
            shoot();
        }
    }
}

function handleKeyUp(e) {
    if (e.code in keys) {
        keys[e.code] = false;
    }
}

// Game loop
function gameLoop(currentTime) {
    if (lastTime === 0) {
        lastTime = currentTime;
    }
    deltaTime = (currentTime - lastTime) / 1000; // Convert to seconds
    lastTime = currentTime;

    if (!gameOver) {
        update();
        render();
        requestAnimationFrame(gameLoop);
    }
}

// Update game state
function update() {
    updateSpaceship();
    updateBullets();
    updateEnemies();
    spawnEnemies();
    checkCollisions();
}

function updateSpaceship() {
    if (keys.ArrowLeft) spaceship.x -= spaceship.speed * deltaTime;
    if (keys.ArrowRight) spaceship.x += spaceship.speed * deltaTime;
    if (keys.ArrowUp) spaceship.y -= spaceship.speed * deltaTime;
    if (keys.ArrowDown) spaceship.y += spaceship.speed * deltaTime;

    // Keep spaceship in bounds
    spaceship.x = Math.max(0, Math.min(spaceship.x, canvas.width - spaceship.width));
    spaceship.y = Math.max(0, Math.min(spaceship.y, canvas.height - spaceship.height));
}

function shoot() {
    const currentTime = Date.now();
    if (currentTime - spaceship.lastShot >= spaceship.fireRate) {
        bullets.push({
            x: spaceship.x + spaceship.width / 2 - 2.5,
            y: spaceship.y,
            width: 5,
            height: 10,
            speed: 500,
            damage: 25
        });
        spaceship.lastShot = currentTime;
    }
}

function updateBullets() {
    // Oyuncu mermilerini güncelle
    for (let i = bullets.length - 1; i >= 0; i--) {
        bullets[i].y -= bullets[i].speed * deltaTime;
        if (bullets[i].y < 0) {
            bullets.splice(i, 1);
        }
    }
    
    // Düşman mermilerini güncelle
    for (let i = enemyBullets.length - 1; i >= 0; i--) {
        const bullet = enemyBullets[i];
        bullet.x += bullet.velocityX * deltaTime;
        bullet.y += bullet.velocityY * deltaTime;
        
        // Ekran dışına çıkan mermileri kaldır
        if (bullet.y > canvas.height || bullet.y < 0 || 
            bullet.x > canvas.width || bullet.x < 0) {
            enemyBullets.splice(i, 1);
        }
    }
}

function spawnEnemies() {
    const currentTime = Date.now();
    if (currentTime - lastEnemySpawn >= ENEMY_SPAWN_INTERVAL) {
        const enemyTypes = ['basic', 'fast', 'strong', 'boss'];
        const type = enemyTypes[Math.floor(Math.random() * enemyTypes.length)];
        
        const enemy = {
            x: Math.random() * (canvas.width - 40),
            y: -40,
            width: type === 'boss' ? 80 : 40,
            height: type === 'boss' ? 80 : 40,
            type: type,
            health: type === 'boss' ? 500 : (type === 'strong' ? 100 : (type === 'basic' ? 50 : 30)),
            speed: type === 'fast' ? 200 : (type === 'boss' ? 80 : 100),
            damage: type === 'boss' ? 30 : (type === 'strong' ? 20 : (type === 'basic' ? 10 : 5)),
            lastShot: 0,
            fireRate: type === 'boss' ? 1000 : (type === 'fast' ? 1500 : 2000), // Mermi atış hızı
            isEnraged: false // Boss için öfke durumu
        };
        
        enemies.push(enemy);
        lastEnemySpawn = currentTime;
    }
}

function updateEnemies() {
    for (let i = enemies.length - 1; i >= 0; i--) {
        const enemy = enemies[i];
        const currentTime = Date.now();
        
        // Hareket mantığı
        if (enemy.type === 'basic') {
            enemy.y += enemy.speed * deltaTime;
        } else if (enemy.type === 'fast') {
            // Hızlı düşman oyuncuya doğru hareket eder
            const dx = spaceship.x - enemy.x;
            const dy = spaceship.y - enemy.y;
            const distance = Math.sqrt(dx * dx + dy * dy);
            
            enemy.x += (dx / distance) * enemy.speed * deltaTime;
            enemy.y += (dy / distance) * enemy.speed * deltaTime;
        } else if (enemy.type === 'strong') {
            // Güçlü düşman yavaş ama kararlı hareket eder
            const dx = spaceship.x - enemy.x;
            const dy = spaceship.y - enemy.y;
            const distance = Math.sqrt(dx * dx + dy * dy);
            
            enemy.x += (dx / distance) * enemy.speed * 0.7 * deltaTime;
            enemy.y += (dy / distance) * enemy.speed * 0.7 * deltaTime;
        } else if (enemy.type === 'boss') {
            // Boss yatay hareket eder
            enemy.x += Math.sin(currentTime / 1000) * enemy.speed * deltaTime;
            enemy.y = Math.min(enemy.y + enemy.speed * 0.2 * deltaTime, canvas.height * 0.2);
            
            // Boss'un sağlığı %30'un altına düştüğünde öfkelenir
            if (!enemy.isEnraged && enemy.health <= 150) {
                enemy.isEnraged = true;
                enemy.fireRate *= 0.7; // Daha hızlı ateş eder
                enemy.speed *= 1.2; // Daha hızlı hareket eder
            }
        }
        
        // Mermi atma mantığı
        if (currentTime - enemy.lastShot >= enemy.fireRate) {
            const dx = spaceship.x - enemy.x;
            const dy = spaceship.y - enemy.y;
            const distance = Math.sqrt(dx * dx + dy * dy);
            
            let bulletSpeed = enemy.type === 'fast' ? 400 : (enemy.type === 'boss' ? 350 : 300);
            let bulletDamage = enemy.damage * 0.5;
            
            if (enemy.type === 'boss' && enemy.isEnraged) {
                // Boss öfkeliyken 3 mermi atar
                for (let angle = -0.2; angle <= 0.2; angle += 0.2) {
                    const dirX = (dx / distance) * Math.cos(angle) - (dy / distance) * Math.sin(angle);
                    const dirY = (dx / distance) * Math.sin(angle) + (dy / distance) * Math.cos(angle);
                    
                    enemyBullets.push({
                        x: enemy.x + enemy.width / 2,
                        y: enemy.y + enemy.height / 2,
                        width: 5,
                        height: 10,
                        velocityX: dirX * bulletSpeed,
                        velocityY: dirY * bulletSpeed,
                        damage: bulletDamage
                    });
                }
            } else {
                // Diğer düşmanlar tek mermi atar
                enemyBullets.push({
                    x: enemy.x + enemy.width / 2,
                    y: enemy.y + enemy.height / 2,
                    width: 5,
                    height: 10,
                    velocityX: (dx / distance) * bulletSpeed,
                    velocityY: (dy / distance) * bulletSpeed,
                    damage: bulletDamage
                });
            }
            
            enemy.lastShot = currentTime;
        }
        
        // Ekran dışına çıkan düşmanları kaldır
        if (enemy.y > canvas.height) {
            enemies.splice(i, 1);
        }
    }
}

function checkCollisions() {
    // Oyuncu mermileri - Düşman çarpışmaları
    for (let i = bullets.length - 1; i >= 0; i--) {
        const bullet = bullets[i];
        for (let j = enemies.length - 1; j >= 0; j--) {
            const enemy = enemies[j];
            if (detectCollision(bullet, enemy)) {
                enemy.health -= bullet.damage;
                bullets.splice(i, 1);
                
                if (enemy.health <= 0) {
                    enemies.splice(j, 1);
                    score += enemy.type === 'boss' ? 1000 : 
                            (enemy.type === 'strong' ? 300 : 
                            (enemy.type === 'basic' ? 100 : 200));
                    updateScore();
                }
                break;
            }
        }
    }
    
    // Düşman mermileri - Oyuncu çarpışmaları
    for (let i = enemyBullets.length - 1; i >= 0; i--) {
        const bullet = enemyBullets[i];
        if (detectCollision(bullet, spaceship)) {
            spaceship.health -= bullet.damage;
            enemyBullets.splice(i, 1);
            updateHealth();
            
            if (spaceship.health <= 0) {
                endGame();
            }
        }
    }

    // Düşman - Oyuncu çarpışmaları
    for (let i = enemies.length - 1; i >= 0; i--) {
        const enemy = enemies[i];
        if (detectCollision(enemy, spaceship)) {
            spaceship.health -= enemy.damage;
            enemies.splice(i, 1);
            updateHealth();
            
            if (spaceship.health <= 0) {
                endGame();
            }
        }
    }
}

function detectCollision(obj1, obj2) {
    return obj1.x < obj2.x + obj2.width &&
           obj1.x + obj1.width > obj2.x &&
           obj1.y < obj2.y + obj2.height &&
           obj1.y + obj1.height > obj2.y;
}

// Rendering
function render() {
    // Clear canvas
    ctx.fillStyle = '#000';
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Draw spaceship
    ctx.fillStyle = '#0F0';
    ctx.fillRect(spaceship.x, spaceship.y, spaceship.width, spaceship.height);

    // Draw player bullets
    ctx.fillStyle = '#FFF';
    bullets.forEach(bullet => {
        ctx.fillRect(bullet.x, bullet.y, bullet.width, bullet.height);
    });

    // Draw enemy bullets
    ctx.fillStyle = '#F00';
    enemyBullets.forEach(bullet => {
        ctx.fillRect(bullet.x, bullet.y, bullet.width, bullet.height);
    });

    // Draw enemies
    enemies.forEach(enemy => {
        // Her düşman türü için farklı renk
        ctx.fillStyle = enemy.type === 'basic' ? '#F00' : 
                       enemy.type === 'fast' ? '#FF0' :
                       enemy.type === 'strong' ? '#F80' :
                       '#F0F'; // Boss mor renkte
        
        // Boss öfkeliyken yanıp sönme efekti
        if (enemy.type === 'boss' && enemy.isEnraged) {
            ctx.fillStyle = Date.now() % 200 < 100 ? '#F0F' : '#FF0';
        }
        
        ctx.fillRect(enemy.x, enemy.y, enemy.width, enemy.height);
    });
}

// UI updates
function updateScore() {
    document.getElementById('currentScore').textContent = score;
}

function updateHealth() {
    document.getElementById('health').textContent = spaceship.health;
}

function endGame() {
    gameOver = true;
    document.getElementById('finalScore').textContent = score;
    document.getElementById('gameOverModal').style.display = 'block';
}

function restartGame() {
    // Reset game state
    spaceship.health = 100;
    spaceship.x = canvas.width / 2 - 25;
    spaceship.y = canvas.height - 100;
    bullets = [];
    enemyBullets = [];
    enemies = [];
    score = 0;
    gameOver = false;
    lastEnemySpawn = 0;
    
    // Update UI
    updateScore();
    updateHealth();
    document.getElementById('gameOverModal').style.display = 'none';
    
    // Restart game loop
    requestAnimationFrame(gameLoop);
}

// Start the game when the page loads
window.addEventListener('load', init);
