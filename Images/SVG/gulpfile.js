var svgstore = require('./index')
var gulp = require('gulp')
var cheerio = require('gulp-cheerio')
var replace = require('gulp-replace');


gulp.task('svg', function () {

  return gulp
    .src('../Icons/*.svg')
    .pipe(cheerio({
      run: function ($) {
        $('[fill="none"]').removeAttr('fill')
      },
      parserOptions: { xmlMode: true }
    }))
    .pipe(svgstore())
    .pipe(replace('svg"><defs>', 'svg">\n\n\n' + 
    '<style type="text/css">\n' +
    '    path:not(.svg-nocolor){fill:currentColor}\n' +
    '    use:not(.svg-nocolor):visited{color:currentColor}\n' +
    '    use:not(.svg-nocolor):hover{color:currentColor}\n' +
    '    use:not(.svg-nocolor):active{color:currentColor}\n' +
    '</style>\n\n\n' + 
    '<defs>'))
	.pipe(gulp.dest('../../App/wwwroot/images/'));
});

//watch task
gulp.task('watch', function () {
    //watch icons folder for SVG file changes from Flash (Animate CC)
    gulp.watch('../Icons/*.svg', gulp.series('svg'));
});
