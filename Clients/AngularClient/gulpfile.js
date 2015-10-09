var fs = require('fs');

fs.stat('./src/environment-settings.js', function(err, stat) {
    if(err == null) {
        console.log('File exists');
    } else {
        throw new Error("Environment settings not found. Please copy environment-settings-template.js and rename to environment-settings.js. This file contains settings for your local environment and will be ignored in version control.");
    }
});

// Setup dependencies
var gulp = require('gulp');
var wiredep = require('wiredep').stream;
var webserver = require('gulp-webserver');
var sass = require('gulp-sass');
var del = require('del');
var uglify = require('gulp-uglify');
var usemin = require('gulp-usemin');
var minifyHtml = require('gulp-minify-html');
var minifyCss = require('gulp-minify-css');
var rev = require('gulp-rev');
var ngAnnotate = require('gulp-ng-annotate');
var environmentSettings = require('./src/environment-settings');

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
    .pipe(inject(gulp.src(['./src/**/*.js', './src/**/*.css', '!./src/bower_components/**/*.*', '!./src/environment-settings-template.js'], {read: false}), {relative: true}))
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
      port: environmentSettings.port,
      host: environmentSettings.host,
      livereload: true,
      directoryListing: false,
      open: true
    }));
});


/******* Build distribution tasks********/


// Remove all content from dist folder
gulp.task('clean', function () {

  return del([
    'dist/**/*',
  ]);
});

// gulp.task('ng-annotate', function () {
//     return gulp.src(['./src/**/*.js', '!./src/bower_components/**/*.*'])
//         .pipe(ngAnnotate())
//         .pipe(gulp.dest('./src'));
// });

gulp.task('minify', ['wireup','clean'], function(){
    
    return gulp.src(['./src/index.html']) // https://github.com/zont/gulp-usemin/issues/89
      
      // When using usemin, make sure the build anchors in the html contain at least one file, otherwise this task will fail silently
      .pipe(usemin({
        vendorcss: [minifyCss(), 'concat', rev()],
        customcss: [minifyCss(), 'concat', rev()],
        html: [minifyHtml({empty: true})],
        vendorjs: [uglify(), rev()],
        customjs: [ngAnnotate(),uglify(), rev()]
      }))     
      .pipe(gulp.dest('./dist'));
});

gulp.task('minify-html', ['minify'], function(){
       // Minify remaining html files
      return gulp.src(['./src/**/*.html', '!./src/index.html'])
        .pipe(minifyHtml({empty: true}))
        .pipe(gulp.dest('./dist'));
});

// Build production environment, output is stored in dist folder
gulp.task('build', ['minify-html'], function(){

  // Copy remaining files to dist folder (eg images, fonts, etc.)
  return gulp.src(['./src/**/*.*', '!./src/**/*.html', '!./src/**/*.js', '!./src/**/*.css', '!./src/**/*.scss', '!./src/bower_components/**/*.*'])
    .pipe(gulp.dest('./dist'));
});

// Setup webserver for production environment
gulp.task('serve-dist', ['build'], function() {
  
  return gulp.src('dist')
    .pipe(webserver({
      port: environmentSettings.port,
      host: environmentSettings.host,
      livereload: false,
      directoryListing: false,
      open: true
    }));
});

