const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: "http://4.178.10.200/",
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
  },
});
