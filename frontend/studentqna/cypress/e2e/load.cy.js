describe("Homepage loads under 2s", () => {
  it("load time < 2s", () => {
    cy.visit("/", {
      onBeforeLoad(win) {
        win.__loadStart = performance.now();
      },
      onLoad(win) {
        const duration = performance.now() - win.__loadStart;
        expect(duration).to.be.lessThan(2000);
      }
    });
  });
});
