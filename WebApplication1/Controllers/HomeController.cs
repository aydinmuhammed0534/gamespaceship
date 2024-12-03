using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static Game _game = new();
        private static float _deltaTime = 1f / 60f; // 60 FPS

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_game);
        }

        public IActionResult Game()
        {
            var gameViewModel = new GameViewModel
            {
                HighScore = _game.GetHighScore() // Assuming GetHighScore() method exists
            };
            return View(gameViewModel);
        }

        [HttpPost]
        public IActionResult MoveLeft()
        {
            _game.Player.MoveLeft(_deltaTime);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MoveRight()
        {
            _game.Player.MoveRight(_deltaTime);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MoveUp()
        {
            _game.Player.MoveUp(_deltaTime);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MoveDown()
        {
            _game.Player.MoveDown(_deltaTime);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Shoot()
        {
            _game.Player.Shoot();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Update()
        {
            _game.Update(_deltaTime);
            return Json(new
            {
                success = true,
                gameState = new
                {
                    playerX = _game.Player.X,
                    playerY = _game.Player.Y,
                    playerHealth = _game.Player.Health,
                    enemies = _game.Enemies.Select(e => new
                    {
                        x = e.X,
                        y = e.Y,
                        type = e.GetType().Name
                    }),
                    score = _game.Score,
                    isGameOver = _game.IsGameOver,
                    isPaused = _game.IsPaused
                }
            });
        }

        [HttpPost]
        public IActionResult TogglePause()
        {
            _game.TogglePause();
            return Json(new { success = true, isPaused = _game.IsPaused });
        }

        [HttpPost]
        public IActionResult NewGame()
        {
            _game = new Game();
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetGameState()
        {
            var enemies = _game.Enemies.Where(e => e.IsActive).Select(e => new
            {
                x = e.X,
                y = e.Y,
                bullets = e.Bullets.Where(b => b.IsActive).Select(b => new
                {
                    x = b.X,
                    y = b.Y
                })
            });


            return Json(new
            {
                score = _game.Score,
                isGameOver = _game.IsGameOver,
                isPaused = _game.IsPaused,
                playerHealth = _game.Player?.Health ?? 0,
                enemies = enemies,
            });
        }

        [HttpPost]
        public IActionResult SaveScore(int score)
        {
            // TODO: Implement score saving logic
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult GetHighScores()
        {
            string path = "highscores.txt";
            List<HighScore> highScores = new();

            if (System.IO.File.Exists(path))
            {
                var lines = System.IO.File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int score))
                    {
                        highScores.Add(new HighScore
                        {
                            PlayerName = parts[0],
                            Score = score
                        });
                    }
                }
            }

            return Json(highScores.OrderByDescending(s => s.Score).Take(10));
        }

        [HttpPost]
        public IActionResult SaveHighScore([FromBody] HighScore score)
        {
            if (score == null || string.IsNullOrWhiteSpace(score.PlayerName))
            {
                return BadRequest();
            }

            string path = "highscores.txt";
            string line = $"{score.PlayerName},{score.Score}";
            System.IO.File.AppendAllLines(path, new[] { line });

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class HighScore
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }
}