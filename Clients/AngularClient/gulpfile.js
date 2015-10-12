// Setup dependencies
var gulp = require('gulp');
var wiredep = require('wiredep').stream;
var webserver = require('gulp-webserver');
var sass = require('gulp-sass');
var ngAnnotate = require('gulp-ng-annotate');

// Setup fonts
gulp.task('copy-fonts', function(){
  return gulp.src(['./src/bower_components/bootstrap-sass/assets/fonts/bootstrap/**/*.*','./src/bower_components/fontawesome/fonts/**/*.*'])
    .pipe(gulp.dest('./src/fonts'));
});

// Wireup bower dependencies
gulp.task('wireup-bower',['copy-fonts'], function () {
  
  return gulp.src(['./src/index.html', './src/styles/sass/main.scss'], {base:'./src'}) // Base property: http://stackoverflow.com/a/24412960/426840
    .pipe(wiredep())
    .pipe(gulp.dest('./src'));
});

// Compile sass files
gulp.task('sass', ['wireup-bower'], function () {
  return gulp.src('./src/styles/sass/**/*.scss')
    .pipe(sass().on('error', sass.logError))
    .pipe(gulp.dest('./src/styles/css'));
});

// Wireup dependencies
gulp.task('wireup', ['sass'], function () {
  
  var inject = require('gulp-inject');
  
  // Wireup all custom js and css (exclude bower files, they are already wired using wiredep)
  return gulp.src('./src/index.html') 
    .pipe(inject(gulp.src(['./src/**/*.js', './src/**/*.css', '!./src/bower_components/**/*.*'], {read: false}), {relative: true}))
    .pipe(gulp.dest('./src'));
});

// Re-compile sass files when changed
gulp.task('sass:watch',['wireup'], function () {
  return gulp.watch('./src/styles/sass/**/*.scss', ['sass']);
});

// Setup webserver for development environment
gulp.task('serve',['sass:watch'], function() {
  return gulp.src('src')
    .pipe(webserver({
      livereload: true,
      directoryListing: false,
      open: true
    }));
});




