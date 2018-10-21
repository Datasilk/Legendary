'use strict';

//includes
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    compile = require('google-closure-compiler-js').gulp(),
    cleancss = require('gulp-clean-css'),
    less = require('gulp-less'),
    rename = require('gulp-rename'),
    merge = require('merge-stream'),
    config = require('./App/config.json');
    
//get config variables from config.json
var environment = config.environment;

//determine environment
var prod = false;
if (environment != 'dev' && environment != 'development' && environment != null) {
    //using staging or production environment
    prod = true;
}

//paths
var paths = {
    scripts: 'App/Scripts/',
    css: 'App/CSS/',
    app: 'App/',
    themes: 'App/Content/themes/',
    vendor: {
        root: 'App/Vendor/**/'
    },
    webroot: 'App/wwwroot/',
};

//working paths
paths.working = {
    js: {
        platform: [
            paths.scripts + 'selector/selector.js',
            paths.scripts + 'utility/velocity.min.js',
            paths.scripts + 'platform/_super.js', // <---- Datasilk Core Js: S object
            paths.scripts + 'platform/ajax.js', //   <---- Optional platform features
            paths.scripts + 'platform/loader.js',
            paths.scripts + 'platform/message.js',
            paths.scripts + 'platform/polyfill.js',
            paths.scripts + 'platform/popup.js', 
            paths.scripts + 'platform/scaffold.js',
            paths.scripts + 'platform/svg.js',
            paths.scripts + 'platform/util.js',
            paths.scripts + 'platform/util.color.js',
            paths.scripts + 'platform/validate.js',
            paths.scripts + 'platform/window.js' //  <---- End of Optional features
        ],
        app: paths.app + '**/*.js',
        utility: [
            paths.scripts + 'utility/*.js',
            paths.scripts + 'utility/**/*.js'
        ],
        core: [
            '!' + paths.scripts + 'core/platform.js',
            paths.scripts + 'core/*.js'
        ]
    },

    less:{
        platform: paths.css + 'platform.less',
        app: [
            paths.app + '**/*.less',
            '!' + paths.css + '**/*.less'
        ],
        themes: paths.css + 'themes/*.less',
        tapestry: paths.css + 'tapestry/tapestry.less',
        utility: paths.css + 'utility/*.less'
    },

    css: {
        utility: paths.css + 'utility/**/*.css',
        themes: paths.themes + '**/*.css',
        app: paths.app + '**/*.css'
    },

    vendor: {
        js: paths.vendor.root + 'js/*.js',
        css: paths.vendor.root + 'css/*.css',
        less: paths.vendor.root + 'css/app.less'
    },

    exclude: {
        app: [
            '!' + paths.app + 'wwwroot/**/*',
            '!' + paths.app + 'Content/**/*',
            '!' + paths.app + 'CSS/**/*',
            '!' + paths.app + 'CSS/*',
            '!' + paths.app + 'Scripts/**/*',
            '!' + paths.app + 'obj/**/*'
        ]
    },

    dashboard: {
        js: [
            paths.scripts + 'utility/simplemde.min.js',
            paths.scripts + 'utility/highlight.min.js',
            paths.scripts + 'utility/remarkable.min.js',
            paths.app + 'views/dashboard/dashboard.js'
        ],
        css: [
            paths.css + 'utility/font-awesome.css',
            paths.css + 'utility/simplemde.min.css',
            paths.css + 'utility/highlight/atelier-forest-light.css', // <-- code syntax highlighting color scheme
            paths.webroot + 'css/views/dashboard/dashboard.css'
        ],
        compiled: paths.webroot + 'css/views/dashboard/dashboard.css'
    }
};

//compiled paths
paths.compiled = {
    platform: paths.webroot + 'js/platform.js',
    js: paths.webroot + 'js/',
    css: paths.webroot + 'css/',
    app: paths.webroot + 'css/',
    themes: paths.webroot + 'css/themes/'
};

//tasks for compiling javascript //////////////////////////////////////////////////////////////
gulp.task('js:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    pathlist.unshift(paths.working.js.app);
    console.log(pathlist);
    var p = gulp.src(pathlist)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));

    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest(paths.compiled.js, { overwrite: true }));
});

gulp.task('js:selector', function () {
    var p = gulp.src(paths.scripts + 'selector/selector.js', { base: '.' })
            .pipe(concat('selector.js'));
    if (prod == true) { 
        //p = p
        //    .pipe(compile({
        //        compilationLevel: 'SIMPLE',
        //        warningLevel: 'VERBOSE',
        //        jsOutputFile: 'selector.js',  // outputs single file
        //        createSourceMap: true
        //    }));
        p = p.pipe(uglify());
    }
    return p.pipe(gulp.dest(paths.compiled.js, { overwrite: true }));
});

gulp.task('js:platform', gulp.series('js:selector'), function () {
    var p = gulp.src(paths.working.js.platform, { base: '.' })
        .pipe(concat(paths.compiled.platform));
    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest('.', { overwrite: true }));
});

gulp.task('js:utility', function () {
    var p = gulp.src(paths.working.js.utility)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));

    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest(paths.compiled.js + 'utility', { overwrite: true }));
});

gulp.task('js:core', function () {
    var p = gulp.src(paths.working.js.core)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));

    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest(paths.compiled.js + 'core', { overwrite: true }));
});

/* custom js compiling */
gulp.task('js:dashboard', function () {
    var p = gulp.src(paths.working.dashboard.js, { base: '.' })
        .pipe(concat(paths.compiled.js + 'dashboard.js'));
    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest('.', { overwrite: true }));
});

gulp.task('js', gulp.series(
    'js:app',
    'js:platform',
    'js:utility',
    'js:core',
    'js:dashboard'
));

//tasks for compiling LESS & CSS /////////////////////////////////////////////////////////////////////
gulp.task('less:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    for (var x = paths.working.less.app.length - 1; x >= 0; x--) {
        pathlist.unshift(paths.working.less.app[x]);
    }
    var p = gulp.src(pathlist)
        .pipe(less())
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if(prod == true){ p = p.pipe(cleancss({compatibility: 'ie8'})); }
    return p.pipe(gulp.dest(paths.compiled.app, { overwrite: true }));
});

gulp.task('less:platform', function () {
    var p = gulp.src(paths.working.less.platform)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css, { overwrite: true }));
});

gulp.task('less:themes', function () {
    var p = gulp.src(paths.working.less.themes)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'themes', { overwrite: true }));
});

gulp.task('less:utility', function () {
    var p = gulp.src(paths.working.less.utility)
        .pipe(less());
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'themes', { overwrite: true }));
});

gulp.task('css:themes', function () {
    var p = gulp.src(paths.working.css.themes)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.themes, { overwrite: true }));
});

gulp.task('css:app', function () {
    var pathlist = paths.working.exclude.app.slice(0);
    pathlist.unshift(paths.working.css.app);
    var p = gulp.src(pathlist)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.app, { overwrite: true }));
});

gulp.task('css:utility', function () {
    var p = gulp.src(paths.working.css.utility)
        .pipe(rename(function (path) {
            path.dirname = path.dirname.toLowerCase();
            path.basename = path.basename.toLowerCase();
            path.extname = path.extname.toLowerCase();
        }));
    if (prod == true) { p = p.pipe(cleancss({ compatibility: 'ie8' })); }
    return p.pipe(gulp.dest(paths.compiled.css + 'utility', { overwrite: true }));
});

/* custom css compiling */
gulp.task('css:dashboard', function () {
    var p = gulp.src(paths.working.dashboard.css, { base: '.' })
        .pipe(concat(paths.compiled.css + 'dashboard.css'));
    if (prod == true) { p = p.pipe(uglify()); }
    return p.pipe(gulp.dest('.', { overwrite: true }));
});

gulp.task('less', gulp.series(
    'less:platform',
    'less:app',
    'less:themes',
    'less:utility'
));

gulp.task('css', gulp.series(
    'css:themes',
    'css:app',
    'css:utility',
    'css:dashboard'
));

//tasks for compiling vendor app dependencies /////////////////////////////////////////////////


//default task
gulp.task('default', gulp.series('js', 'less', 'css'));

//watch task
gulp.task('watch', function () {
    //watch platform JS
    gulp.watch([
        paths.scripts + 'selector/selector.js',
        paths.scripts + 'core/platform.js',
        paths.scripts + 'platform/*.js'
    ], gulp.series('js:platform'));

    //watch core JS
    gulp.watch(paths.working.js.core, gulp.series('js:core'));

    //watch app JS
    var pathjs = paths.working.exclude.app.slice(0);
    for (var x = 0; x < pathjs.length; x++) {
        pathjs[x] += '*.js';
    }
    pathjs.unshift(paths.working.js.app);
    gulp.watch(pathjs, gulp.series('js:app'));

    //watch dashboard JS
    gulp.watch(paths.working.dashboard.js, gulp.series('js:dashboard'));

    //watch app LESS
    var pathless = paths.working.exclude.app.slice(0);
    for (var x = 0; x < pathless.length; x++) {
        pathless[x] += '*.less';
    }
    for (var x = paths.working.less.app.length - 1; x >= 0; x--) {
        pathless.unshift(paths.working.less.app[x]);
    }
    gulp.watch(pathless, gulp.series('less:app', 'css:dashboard'));

    //watch platform LESS
    gulp.watch([
        paths.working.less.platform,
        paths.working.less.tapestry
    ], gulp.series('less:platform'));

    //watch themes LESS
    gulp.watch([
        paths.working.less.themes
    ], gulp.series('less:themes', 'less:platform'));

    //watch utility LESS
    gulp.watch([
        paths.working.less.utility
    ], gulp.series('less:utility'));

    //watch app CSS
    var pathcss = paths.working.exclude.app.slice(0);
    for (var x = 0; x < pathcss.length; x++) {
        pathcss[x] += '*.css';
    }
    pathcss.unshift(paths.working.css.app);
    gulp.watch(pathcss, gulp.series('css:app'));

    //watch themes CSS
    gulp.watch([
        paths.working.css.themes
    ], gulp.series('css:themes'));

    //watch utility CSS
    gulp.watch([
        paths.working.css.utility
    ], gulp.series('css:utility'));
});