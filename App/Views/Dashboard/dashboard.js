/* dashboard */
S.dash = {
    init: function () {
        //buttons
        $('.btn-newbook').on('click', S.books.create.view);
        $('.item-trash > a').on('click', S.trash.show);

        //events
        $(window).on('resize', S.entries.resize);

        //scrollbar
        $('.subbar .entries .scrollbar').on('mousedown', S.entries.scroll.start);
        $('.subbar .entries .scroller').on('mousedown', S.entries.scroll.bar);
        $('.subbar .entries .container').on('wheel', S.entries.scroll.wheel);
        S.entries.resize();

        //init editor
        S.editor.init();
    },

    hideAll: function () {
        $('.sidebar > .menu li.selected').removeClass('selected');
        $('.editor, .tags, .trash').hide();
    }
};

/* Books */
S.books = {
    create: {
        view: function() {
            var scaffold = new S.scaffold($('#template_newbook').html());
            S.popup.show('Create a new Book', scaffold.render(), { width: 350 });
            $('.popup form').on('submit', S.books.create.submit);
        },

        submit: function (e) {
            e.preventDefault();
            e.cancelBubble = true;
            var data = { title: $('#txtbook_title').val() };
            if (data.title == '') {
                S.message.show('.popup .message', 'error', 'Please provide a title for your new book');
                return false;
            }

            S.ajax.post('Books/CreateBook', data, function (d) {
                if (d.indexOf('success') == 0) {
                    var f = d.split('|');
                    $('.menu .sub.book').remove();
                    $('.menu .item-books').after(f[2]);
                    S.popup.hide();
                    S.entries.view(f[1]);
                } else {
                    S.message.show('.popup .message', 'error', d);
                }
            });

            return false;
        }
    }
};

/* Entries */
S.entries = {
    bookId: 0,

    resize: function () {
        //resize entries height
        const win = S.window.pos();
        const container = $('.subbar .entries .container');
        const scrollbar = container.find('.scrollbar');
        const pos = container.position();
        const height = win.h - pos.top;
        container.css({ height: height });

        //show/hide entries scrollbar 
        const entries = $('.subbar .entry');
        let h = 0;
        for (let x = 0; x < entries.length; x++) {
            h += $(entries[x]).height();
        }
        if (h > win.h - pos.top) {
            //show scrollbar
            if (!container.hasClass('scroll')) {
                container.addClass('scroll');
            }
            //update scrollbar height
            container.find('.scroller').css({ height: height - 7 });
            scrollbar.css({ height: ((height - 7) / h) * (height) });

            //update scroll info
            S.entries.scroll.init();
        } else {
            //hide scrollbar
            if (container.hasClass('scroll')) {
                container.removeClass('scroll');
            }
        }
    },

    scroll: {
        config: {skip:15},
        selected: { scrollable: null, height: null, itemsH: null },

        init: function () {
            const win = S.window.pos();
            const container = $('.subbar .entries .container');
            const scrollbar = container.find('.scrollbar');
            const scrolled = $('.subbar .entries .scrolled');
            const entries = $('.subbar .entry');
            const scroller = container.find('.scroller');
            const pos = container.position();
            const height = win.h - pos.top;
            container.css({ height: height });

            //show/hide entries scrollbar
            let h = 0;
            for (let x = 0; x < entries.length; x++) {
                h += $(entries[x]).height();
            }

            S.entries.scroll.selected = {
                scrollbar: scrollbar,
                height: height,
                barHeight: ((height) / h) * height,
                container: container,
                entries: entries,
                scrolled: scrolled,
                entriesH: h,
                offsetY: scroller.offset().top,
                barY: scrollbar.offset().top
            };
        },

        start: function (e) {
            e.cancelBubble = true;
            e.stopPropagation();
            e.preventDefault();

            //update class for container
            const container = $('.subbar .entries .container');
            container.addClass('scrolling');
            S.entries.scroll.init();
            S.entries.scroll.selected.cursorY = e.clientY;
            S.entries.scroll.selected.currentY = e.clientY;

            $('body').on('mousemove', S.entries.scroll.move);
            $('body').on('mouseup', S.entries.scroll.stop);
            S.entries.scroll.animate.call(S.entries.scroll);
        },

        move: function (e) {
            S.entries.scroll.selected.currentY = e.clientY;
        },

        animate: function () {
            const scroll = S.entries.scroll.selected;
            const curr = scroll.currentY - scroll.cursorY - (scroll.offsetY - scroll.barY);
            let perc = (100 / (scroll.height - scroll.barHeight)) * curr;
            S.entries.scroll.to(perc);
            requestAnimationFrame(() => {
                S.entries.scroll.animate.call(S.entries.scroll);
            });
        },

        stop: function () {
            $('body').off('mousemove', S.entries.scroll.move);
            $('body').off('mouseup', S.entries.scroll.stop);
            S.entries.scroll.selected.container.removeClass('scrolling');
        },

        to: function (percent) {
            const scroll = S.entries.scroll.selected;
            let perc = S.math.clamp(percent, 0, 100);
            scroll.scrollbar.css({ top: ((scroll.height - scroll.barHeight) / 100) * perc });
            scroll.scrolled.css({ top: -1 * (((scroll.entriesH - scroll.height) / 100) * perc) });
        },

        add: function (px) {
            S.entries.scroll.init();
            const scroll = S.entries.scroll.selected;
            const scrolled = $('.subbar .entries .scrolled');
            let pos = scrolled.position();
            let perc = (100 / (scroll.entriesH - scroll.height)) * -(pos.top + px);
            S.entries.scroll.to(perc);
        },

        bar: function (e) {
            if ($(e.target).hasClass('scrollbar')) { return;}
            const y = e.clientY;
            const scrollbar = $('.entries .scrollbar');
            const pos = scrollbar.offset();
            if (y < pos.top) {
                //above scrollbar
                S.entries.scroll.add(S.entries.scroll.config.skip * 8);
            } else {
                //below scrollbar
                S.entries.scroll.add(-S.entries.scroll.config.skip * 8);
            }
        },

        wheel: function (e) {
            let delta = 0;
            if (!e) e = window.event;
            // normalize delta
            if (e.wheelDelta) {
                // IE and Opera
                delta = e.wheelDelta / 60;
            } else if (e.detail) {
                // W3C
                delta = -e.detail / 2;
            }
            S.entries.scroll.add(delta * S.entries.scroll.config.skip);
        }
    },

    view: function (id) {
        var data = { bookId: id, start: 1, length: 50, sort: 0, includeCount:true };
        S.ajax.post('Entries/GetList', data, function (d) {
            var f = d.split('|');
            if (f.length > 1) {
                $('ul.menu li.book.selected').removeClass('selected');
                $('ul.menu li.book.id-' + id).addClass('selected');
                $('.subbar .entries').html(f[1]);
                $('.subbar').removeClass('hide');
                S.popup.hide();
                if (parseInt(f[0]) > 0) {
                    $('.editor').removeClass('hide');
                } else {
                    S.entries.noentries();
                }
            }
            
            S.entries.bookId = id;
        });
    },

    create: {
        temp: { title: '', summary: '' },

        view: function (callback) {
            var scaffold = new S.scaffold($('#template_newentry').html());
            S.popup.show('Create a new Entry', scaffold.render(), { width: 350 });
            $('.popup form').on('submit', S.entries.create.submit);
            //get list of chapters
            S.entries.create.getChapters(callback);  
        },

        submit: function (e) {
            e.preventDefault();
            e.cancelBubble = true;
            var data = {
                bookId: S.entries.bookId,
                title: $('#txtentry_title').val(),
                summary: $('#txtentry_summary').val(),
                chapter: parseInt($('#lstentry_chapter').val()),
                sort: 0
            };
            if (data.title == '') {
                S.message.show('.popup .message', 'error', 'Please provide a title for your new entry');
                return false;
            }
            if (data.summary == '') {
                S.message.show('.popup .message', 'error', 'Please provide a summary for your new entry');
                return false;
            }

            S.ajax.post('Entries/CreateEntry', data, function (d) {
                if (d.indexOf('|') > 0) {
                    var f = d.split('|');
                    $('.subbar .entries').html(f[1]);
                    $('.editor').removeClass('hide');
                    $('.no-entries').remove();
                    S.popup.hide();
                    S.editor.setContent('');
                    S.editor.entryId = parseInt(f[0]);
                } else {
                    S.message.show('.popup .message', 'error', d);
                }
            });

            return false;
        },

        getChapters: function (callback) {
            S.ajax.post('Chapters/GetList', { bookId: S.entries.bookId }, function (d) {
                if (d.indexOf('[') == 0) {
                    var data = JSON.parse(d);
                    data = [{ title: '[No Chapter]', num: 0 }].concat(data);
                    var list = $('#lstentry_chapter');
                    list.html('');
                    data.map(a => {
                        list.append(new Option((a.num > 0 ? a.num + ': ' : '') + a.title, a.num));
                    });
                    if (typeof callback == 'function') { callback();}
                } else {
                    S.message.show('.popup .message', 'error', 'An error occurred while trying to retrieve a list of chapters');
                }
            });
        },

        newChapter: function () {
            S.entries.create.temp = {
                title: $('#txtentry_title').val(),
                summary: $('#txtentry_summary').val()
            };
            S.chapters.create.view(S.entries.create.createdChapter);
        },

        createdChapter: function (chapter) {
            var temp = S.entries.create.temp;
            S.entries.create.view(function () {
                $('#lstentry_chapter').val(chapter);
            });
            $('#txtentry_title').val(temp.title);
            $('#txtentry_summary').val(temp.summary);
        }
    },

    noentries: function () {
        $('.editor').addClass('hide');
        $('.no-entries').remove();
        $('.editor').after($('#template_noentries').html());
        $('.btn-newentry').on('click', S.entries.create.view);
    }
};

/* Chapters */
S.chapters = {
    create: {
        callback: null,

        view: function (callback) {
            var scaffold = new S.scaffold($('#template_newchapter').html());
            S.popup.show('Create a new Chapter', scaffold.render(), { width: 400 });
            $('.popup form').on('submit', S.chapters.create.submit);
            //get max chapter # for book
            S.ajax.post('Chapters/GetMax', { bookId: S.entries.bookId }, function (d) {
                try {
                    $('#txtchapter_num').val(parseInt(d) + 1);
                } catch (ex) {
                    S.message.show('.popup .message', 'error', S.message.error.generic);
                }
            });
            S.chapters.create.callback = callback;
        },

        cancel: function () {
            S.popup.hide();
            if (typeof S.chapters.create.callback == 'function') {
                S.chapters.create.callback(0);
            }
        },

        submit: function (e) {
            e.preventDefault();
            e.cancelBubble = true;
            try {
                var data = {
                    bookId: S.entries.bookId,
                    chapter: parseInt($('#txtchapter_num').val()),
                    title: $('#txtchapter_title').val(),
                    summary: $('#txtchapter_summary').val()
                };
                if (data.title == '') {
                    S.message.show('.popup .message', 'error', 'Please provide a title for your new chapter');
                    return false;
                }

                S.ajax.post('Chapters/CreateChapter', data, function (d) {
                    if (d.indexOf('success') == 0) {
                        S.popup.hide();
                        if (typeof S.chapters.create.callback == 'function') {
                            S.chapters.create.callback(data.chapter);
                        }
                    } else {
                        S.message.show('.popup .message', 'error', d);
                    }
                });
            } catch (ex) {
                S.message.show('.popup .message', 'error', S.message.error.generic);
            }
            return false;
        }
    }
};

/* Editor */
var editor;
var markdown;
S.editor = {
    entryId: null,
    div: null,

    init: function () {
        //initialize markdown renderer (with code syntax highlighting support)
        markdown = new Remarkable({
            highlight: function (str, lang) {
                var language = lang || 'javascript';
                if (language && hljs.getLanguage(language)) {
                    try {
                        return hljs.highlight(language, str).value;
                    } catch (err) { }
                }
                try {
                    return hljs.highlightAuto(str).value;
                } catch (err) { }
                return '';
            }
        });

        //initialize markdown editor
        editor = new SimpleMDE({
            element: document.getElementById("editor"),
            placeholder: 'Start writing here while using markdown syntax!',
            forceSync: true,
            spellChecker: false,
            status: false,
            tabSize: 4,
            toolbar: [
                {
                    name: "bold",
                    action: SimpleMDE.toggleBold,
                    className: "fa fa-bold",
                    title: "Bold (Ctrl+B)",
                },
                {
                    name: "italic",
                    action: SimpleMDE.toggleItalic,
                    className: "fa fa-italic",
                    title: "Italic (Ctrl+I)",
                },
                {
                    name: "strikethrough",
                    action: SimpleMDE.toggleStrikethrough,
                    className: "fa fa-strikethrough",
                    title: "Strikethrough (Ctrl+S)",
                },
                {
                    name: "heading",
                    action: SimpleMDE.toggleHeadingSmaller,
                    className: "fa fa-header",
                    title: "Heading (Ctrl+H)",
                },
                {
                    name: "quote",
                    action: SimpleMDE.toggleBlockquote,
                    className: "fa fa-quote-left",
                    title: "Quote",
                },
                "|",
                {
                    name: "unordered-list",
                    action: SimpleMDE.toggleUnorderedList,
                    className: "fa fa-list-ul",
                    title: "Generic List (Ctrl+L)",
                },
                {
                    name: "ordered-list",
                    action: SimpleMDE.toggleOrderedList,
                    className: "fa fa-list-ol",
                    title: "Numbered List (Ctrl+Alt+L)",
                },
                "|",
                {
                    name: "link",
                    action: SimpleMDE.drawLink,
                    className: "fa fa-link",
                    title: "Create Link",
                },
                {
                    name: "image",
                    action: SimpleMDE.drawImage,
                    className: "fa fa-picture-o",
                    title: "Insert Image",
                },
                {
                    name: "code",
                    action: SimpleMDE.toggleCodeBlock,
                    className: "fa fa-code",
                    title: "Code (Ctrl+Alt+C)",
                },
                "|",
                {
                    name: "table",
                    action: SimpleMDE.drawTable,
                    className: "fa fa-table",
                    title: "Insert Table",
                },
                {
                    name: "horizontal-rule",
                    action: SimpleMDE.drawHorizontalRule,
                    className: "fa fa-minus",
                    title: "Insert Horizontal Line",
                },
                "|",
                {
                    name: "guide",
                    action: S.editor.guide.show,
                    className: "fa fa-question-circle",
                    title: "Markdown Guide",
                },
                {
                    name: "preview",
                    action: SimpleMDE.togglePreview,
                    className: "fa fa-eye no-disable",
                    title: "Toggle Preview",
                },
                "|",
                {
                    name: "side-by-side",
                    action: SimpleMDE.toggleSideBySide,
                    className: "fa fa-columns no-disable no-mobile",
                    title: "Toggle Side by Side",
                },
                {
                    name: "fullscreen",
                    action: SimpleMDE.toggleFullScreen,
                    className: "fa fa-arrows-alt no-disable no-mobile",
                    title: "Toggle Fullscreen",
                },
                "|",
                {
                    name: "info",
                    action: S.editor.info.show,
                    className: "fa fa-exclamation-circle no-disable",
                    title: "Information About this Entry",
                },

            ],
            renderingConfig: {
                codeSyntaxHighlighting: true
            },
            autoDownloadFontAwesome: false,
            autofocus: true,
            previewRender: function (text) {
                return markdown.render(text)
                    .replace('<code>', '<code class="hljs">'); //bug fix
            }
        });

        S.editor.div = $('.markdown-editor');

        //set up event to detect changes to editor
        setTimeout(function () {
            editor.codemirror.on('change', S.editor.updated.check);
        }, 1000);

        //set up window resize event
        $(window).on('resize', S.editor.resize);
        S.editor.resize();
    },

    resize: function () {
        var win = S.window.pos();
        var pos = S.editor.div.offset();
        S.editor.div.css({ height: win.h - pos.top - win.scrolly });
    },

    getContent: function (entryId) {
        if (editor.value() != '' && S.editor.changed == true) { S.editor.save(); }
        S.editor.setContent('');
        S.editor.entryId = entryId;
        S.ajax.post('Entries/LoadEntry', { entryId: entryId, bookId: S.entries.bookId }, function (d) {
            if (d == 'error') {
                S.message.show('.editor .message', 'error', 'An error occurred while trying to load your entry');
            } else {
                S.editor.setContent(d);
            }
        });
    },

    setContent: function (content) {
        editor.codemirror.off('change', S.editor.updated.check);

        editor.value(content || '');
        $('#editor').val(content || '');
        if (editor.isPreviewActive() == true) {
            editor.togglePreview();
            setTimeout(() => editor.togglePreview(), 10);
        }

        //set up event to detect changes to editor
        editor.codemirror.on('change', S.editor.updated.check);
    },

    changed: false,

    updated: {
        timer: null,

        check: function () {
            S.editor.changed = true;
            clearTimeout(S.editor.updated.timer);
            S.editor.updated.timer = setTimeout(function () { S.editor.save(); }, 5000);
        }
    },

    save: function () {
        S.editor.changed = false;
        var data = {
            entryId: S.editor.entryId,
            content: editor.value()
        };
        S.ajax.post('Entries/SaveEntry', data, function (d) {
            if (d.indexOf('success') == 0) {

            } else {
                S.message.show('.editor .message', 'error', 'An error occurred while trying to save your entry');
            }
        });
    },

    guide: {
        show: function () {

        },
        hide: function () {

        }
    },

    info: {
        show: function () {

        },
        hide: function () {

        }
    }
};

/* Tags */
S.tags = {
    show: function () {
        S.dash.hideAll();
        $('.tags').show();
    }
};

/* Trash */
S.trash = {
    show: function () {
        S.dash.hideAll();
        $('.sidebar > .menu .item-trash').addClass('selected');
        $('.trash').show();
        S.ajax.post('Trash/LoadTrash', function (d) {
            if (d == 'error') {
                S.message.show('.editor .message', 'error', 'An error occurred while trying to load your trash bin');
            } else {
                S.editor.setContent(d);
            }
        });
    }
};
