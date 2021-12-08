const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target: "https://localhost:7296/",
    secure: false,
    pathRewrite: { '^/weatherforecast': '' }
  }
]

module.exports = PROXY_CONFIG;
