import { CourseTrackerTemplatePage } from './app.po';

describe('CourseTracker App', function() {
  let page: CourseTrackerTemplatePage;

  beforeEach(() => {
    page = new CourseTrackerTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
