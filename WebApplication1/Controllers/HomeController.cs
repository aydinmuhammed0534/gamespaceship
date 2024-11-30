using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Spaceship _player;
        private static List<Enemy> _enemies = new();

        
        // Static constructor ekleyelim
        static HomeController()
        {
            _player = new Spaceship(100, 10, 5); // Sağlık, hasar, hız
            SpawnEnemies(); // Düşmanları oluştur

        }
        private static void SpawnEnemies()
        {
            _enemies.Add(new BasicEnemy()); 
            _enemies.Add(new BossEnemy());
            _enemies.Add(new FastEnemy()); 
            _enemies.Add(new StrongEnemy()); 
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            
            // Eğer _player null ise yeniden oluştur
            if (_player == null)
            {
                _player = new Spaceship(100, 10, 5);
            }
        }

        // Ana oyun ekranını render et
        public IActionResult Index()
        {
            try 
            {
                if (_player == null)
                {
                    _player = new Spaceship(100, 10, 5);
                }
                return View(_player);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Index hatası: {ex.Message}");
                return RedirectToAction("Error", new { message = "Oyun başlatılırken bir hata oluştu!" });
            }
            
            
            
        }

        // Uzay aracını hareket ettirme
        public IActionResult MovePlayer(string direction)
        {
            try
            {
                if (_player == null)
                {
                    _player = new Spaceship(100, 10, 5);
                }

                _logger.LogInformation($"Moving {direction}");
                _player.Move(direction);
                _logger.LogInformation($"Moved {direction}");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hareket hatası: {ex.Message}");
                return RedirectToAction("Error", new { message = "Hareket sırasında bir hata oluştu!" });
            }
        }
        

        // Mermi atma işlemi
        public IActionResult Shoot()
        {
            try
            {
                if (_player == null)
                    return RedirectToAction("Error", new { message = "Oyuncu bulunamadı!" });

                _player.Shoot();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ateş etme hatası: {ex.Message}");
                return RedirectToAction("Error", new { message = "Ateş etme sırasında bir hata oluştu!" });
            }
        }

        // Hasar alma işlemi
        public IActionResult TakeDamage(int amount)
        {
            try
            {
                if (_player == null)
                    return RedirectToAction("Error", new { message = "Oyuncu bulunamadı!" });

                if (amount < 0)
                    return RedirectToAction("Error", new { message = "Geçersiz hasar miktarı!" });

                _player.TakeDamage(amount);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hasar alma hatası: {ex.Message}");
                return RedirectToAction("Error", new { message = "Hasar alma sırasında bir hata oluştu!" });
            }
        }

        // Hata sayfası
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}