'use strict';

// DEPENDENCIES
var path = require('path');
var gulp = require('gulp');
var del = require('del');
var install = require('gulp-install');
var sass = require('gulp-sass');
var sourcemaps = require('gulp-sourcemaps');
var through = require('through2').obj;

// OPTIONS
var CUSTOM_VARS_PATH = path.resolve(__dirname, 'Content/sass/_materialize.variables.scss');

// CLEANS PREVIOUS MATERIALIZE BUILDS
gulp.task('materialize:clean', function() {
    return del('Content/vendor/Materialize/css/*');
});

// INSTALL MATERIALIZE BOWER PACKAGE
gulp.task('materialize:install', function() {
    return gulp.src('bower.json')
        .pipe(install());
});

// BUILD MATERIALIZE WITH CUSTOM VARIABLES INJECTING
gulp.task('materialize:build', function() {
    var origPath = path.resolve(__dirname, 'bower_components/Materialize/sass/materialize.scss');

    var dirname = path.dirname(CUSTOM_VARS_PATH);
    var basename = path.basename(CUSTOM_VARS_PATH, '.scss')
        .replace(/^_(.*)/, '$1');
    var injectorPath = path.resolve(dirname, basename);

    return gulp.src(origPath)
        .pipe(through(function(file, enc, callback) {

            // inject custom variables
            var contents = file.contents.toString()
                .replace('components/variables', injectorPath.replace(/\\/g, '/'));

            file.contents = new Buffer(contents, 'utf8');

            callback(null, file);
        }))
        .pipe(sourcemaps.init())
        .pipe(sass().on('error', sass.logError))
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('./Content/vendor/Materialize/css'));
});

// REMOVE BOWER_COMPONENTS
gulp.task('bower:clean', function() {
    return del('bower_components');
});

// DEFAULT TASK (ALL TASKS IN RIGHT ORDER)
gulp.task('default', gulp.series('bower:clean', 'materialize:clean', 'materialize:install', 'materialize:build', 'bower:clean'));
