'use strict';

//includes
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    compile = require('google-closure-compiler-js').gulp(),
    cleancss = require('gulp-clean-css'),
    less = require('gulp-less'),
    rename = require('gulp-rename'),
    merge = require('merge-stream')
    
var modules = [
    "_super.js",
    "ajax.js",
    "loader.js",
    "message.js",
    "polyfill.js",
    "popup.js",
    "scaffold.js",
    "util.js",
    "util.color.js",
    "validate.js",
    "window.js"
];
gulp.task('js', function () {
    var p = gulp.src(modules, { base: '.' })
        .pipe(concat('dist/platform.js'));
    return p.pipe(gulp.dest('.', { overwrite: true }));
});

//default task
gulp.task('default', ['js']);

//watch task
gulp.task('watch', function () {
    //watch platform JS
    gulp.watch(modules, ['js']);
});