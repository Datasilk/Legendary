﻿/* dashboard */
S.dash = {
    init: function () {
        //buttons
        $('.btn-newbook').on('click', S.books.create.view);
        $('.item-trash > a').on('click', S.trash.view);
        $('.top-menu li:nth-child(1)').on('click', S.menus.books.show);
        $('.top-menu li:nth-child(2)').on('click', S.menus.chapters.show);
        $('.top-menu li:nth-child(3)').on('click', S.menus.page.show);

        //events
        $(window).on('resize', S.entries.resize);

        //init entries
        S.entries.init();

        //init editor
        S.editor.init();
    },

    hideAll: function () {
        $('.sidebar > .menu li.selected').removeClass('selected');
        $('.body, .entries, .tags, .trash').addClass('hide');
    }
};

/* Books */
S.books = {
    create: {
        view: function() {
            var view = new S.view($('#template_newbook').html());
            S.popup.show('Create a new Book', view.render(), { width: 350 });
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

            S.ajax.post('Books/CreateBook', data,
                function (d) {
                    var f = d.split('|');
                    $('.menu .sub.book').remove();
                    $('.menu .item-books').after(f[2]);
                    S.popup.hide();
                    S.entries.view(f[1]);
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );

            return false;
        }
    }
};

/* Entries */
S.entries = {
    bookId: 0,

    init: function () {
        S.entries.bindEvents();
        S.entries.resize();
        S.scrollbar.add($('.entries .container'), {
            touch: true, moved: S.entries.resize, touchEnd: S.entries.resize });
    },

    bindEvents: function () {
        //bind events to chapters list
        $('.chapter .expander').on('click', (e) => {
            var chapter = $(e.target).parents('.chapter').first();
            var chapterId = chapter.attr('data-id');
            if (chapter.hasClass('expanded')) {
                //hide chapter entries
                $('.entry.chapter-' + chapterId).hide();
                chapter.removeClass('expanded');
            } else {
                //show chapter entries
                $('.entry.chapter-' + chapterId).show();
                chapter.addClass('expanded');
            }
        });
    },

    resize: function () {
        //resize entries height
        const win = S.window.pos();
        const container = $('.subbar .entries .container');
        const pos = container[0].getBoundingClientRect();
        container.css({ height: win.h - pos.top - 1 });
    },

    view: function (id, reload) {
        if (id == S.entries.bookId && reload !== true) {
            S.dash.hideAll();
            $('ul.menu li.book.id-' + id).addClass('selected');
            $('.subbar, .subbar .entries').removeClass('hide');
            $('.editor').removeClass('hide');
            const win = S.window.pos();
            if ($('.sidebar.show-card').length > 0 && win.w <= 895) {
                //mobile view, show chapters
                S.menus.chapters.show();
            }
        } else {
            //load list of entries
            var data = { bookId: id, entryId: S.editor.entryId || 0, start: 1, length: 500, sort: 0};
            S.ajax.post('Entries/GetList', data,
                function (d) {
                    S.dash.hideAll();
                    $('ul.menu li.book.selected').removeClass('selected');
                    $('ul.menu li.book.id-' + id).addClass('selected');
                    $('.subbar .entries').html(d);
                    $('.subbar, .subbar .entries').removeClass('hide');
                    S.popup.hide();
                    if($('.entries .entry').length > 0) {
                        //load selected entry
                        $('.editor').removeClass('hide');
                        var entry = $('.entries .entry.selected');
                        if (entry.length > 0) {
                            let entryId = S.entries.getId(entry);
                            S.editor.getContent(entryId);
                        }
                    } else {
                        //no entries exist
                        S.entries.noentries();
                    }

                    S.entries.init();
                    S.entries.bookId = id;
                    const win = S.window.pos();
                    if ($('.sidebar.show-card').length > 0 && win.w <= 895) {
                        //mobile view, show chapters
                        S.menus.chapters.show();
                    }
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );
        }
        
    },

    create: {
        temp: { title: '', summary: '' },

        view: function (callback) {
            var view = new S.view($('#template_newentry').html());
            S.popup.show('Create a new Entry', view.render(), { width: 350 });
            $('.popup form').on('submit', S.entries.create.submit);
            //get list of chapters
            S.chapters.get($('#lstentry_chapter'), callback);  
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

            S.ajax.post('Entries/CreateEntry', data,
                function (d) {
                    var f = d.split('|');
                    $('.subbar .entries').html(f[1]);
                    $('.editor').removeClass('hide');
                    $('.no-entries').remove();
                    S.popup.hide();
                    S.editor.entryId = parseInt(f[0]);
                    S.editor.setContent('# ' + data.title + '\n#### ' + data.summary + '\n\n');
                    S.entries.init();
                    //scroll to bottom of entries list
                    S.scrollbar.to(S.scrollbar.get($('.entries .container')), 100);
                    //save new entry with default content (title/summary)
                    S.editor.save();
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );

            return false;
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

    trash: function (id) {
        if (confirm('Do you really want to send this entry to the trash?\n' +
            'You will be able to restore it later if you need to.')) {
            S.ajax.post('Entries/TrashEntry', { entryId: id },
                function (d) {
                    let entry = $('.entries .entryid-' + id);
                    //get next entry
                    let nextEntry = entry.prev('.entry');
                    if (nextEntry.length == 0) {
                        nextEntry = entry.next('.entry');
                    }
                    if (nextEntry.length == 0) {
                        //no more entries
                        S.entries.noentries();
                        $('.entries .movable > *').remove();
                    } else {
                        //select next entry
                        let nextId = S.entries.getId(nextEntry);
                        S.editor.getContent(nextId);
                    }
                    //remove trashed entry from list
                    entry.remove();
                    S.popup.hide();
                    //update trash count
                    $('.item-trash .count').html('(' + d + ')');
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );
        }
    },

    getId: function (elem) {
        return parseInt($(elem).attr('class')
            .replace('row ', '')
            .replace('hover ', '')
            .replace('entry ', '')
            .replace('entryid-', '')
            .replace('selected', '')
            .trim());
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
    get: function (list, callback) {
        S.ajax.post('Chapters/GetList', { bookId: S.entries.bookId },
            function (d) {
                var data = JSON.parse(d);
                data = [{ title: '[No Chapter]', num: 0 }].concat(data);
                list.html('');
                data.map(a => {
                    list.append(new Option((a.num > 0 ? a.num + ': ' : '') + a.title, a.num));
                });

                if (typeof callback == 'function') { callback(); }
            },
            function (err) {
                S.message.show('.popup .message', 'error', err);
            }
        );
    },

    create: {
        callback: null,

        view: function (callback) {
            var view = new S.view($('#template_newchapter').html());
            S.popup.show('Create a new Chapter', view.render(), { width: 400 });
            $('.popup form').on('submit', S.chapters.create.submit);
            //get max chapter # for book
            S.ajax.post('Chapters/GetMax', { bookId: S.entries.bookId },
                function (d) {
                    $('#txtchapter_num').val(parseInt(d) + 1);
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );
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

                S.ajax.post('Chapters/CreateChapter', data,
                    function (d) {
                        S.popup.hide();
                        if (typeof S.chapters.create.callback == 'function') {
                            S.chapters.create.callback(data.chapter);
                        }
                    },
                    function (err) {
                        S.message.show('.popup .message', 'error', err);
                    }
                );
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
    toggle: {fullscreen:false, preview:false, sidebyside:false},

    init: function () {
        //initialize markdown renderer (with code syntax highlighting support)
        markdown = new Remarkable({
            breaks: true,
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
        markdown.use(S.editor.image.render);

        //initialize markdown editor
        editor = new EasyMDE({
            element: document.getElementById("editor"),
            placeholder: 'Start writing here while using markdown syntax!',
            forceSync: true,
            spellChecker: false,
            status: false,
            tabSize: 4,
            toolbar: [
                {
                    name: "bold",
                    action: EasyMDE.toggleBold,
                    className: "fa fa-bold",
                    title: "Bold (Ctrl+B)",
                },
                {
                    name: "italic",
                    action: EasyMDE.toggleItalic,
                    className: "fa fa-italic",
                    title: "Italic (Ctrl+I)",
                },
                {
                    name: "strikethrough",
                    action: EasyMDE.toggleStrikethrough,
                    className: "fa fa-strikethrough",
                    title: "Strikethrough (Ctrl+S)",
                },
                {
                    name: "heading",
                    action: EasyMDE.toggleHeadingSmaller,
                    className: "fa fa-header",
                    title: "Heading (Ctrl+H)",
                },
                {
                    name: "quote",
                    action: EasyMDE.toggleBlockquote,
                    className: "fa fa-quote-left",
                    title: "Quote",
                },
                "|",
                {
                    name: "unordered-list",
                    action: EasyMDE.toggleUnorderedList,
                    className: "fa fa-list-ul",
                    title: "Generic List (Ctrl+L)",
                },
                {
                    name: "ordered-list",
                    action: EasyMDE.toggleOrderedList,
                    className: "fa fa-list-ol",
                    title: "Numbered List (Ctrl+Alt+L)",
                },
                "|",
                {
                    name: "link",
                    action: EasyMDE.drawLink,
                    className: "fa fa-link",
                    title: "Create Link",
                },
                {
                    name: "image",
                    action: S.editor.image.showDialog,
                    className: "fa fa-picture-o",
                    title: "Insert Image",
                },
                {
                    name: "code",
                    action: EasyMDE.toggleCodeBlock,
                    className: "fa fa-code",
                    title: "Code (Ctrl+Alt+C)",
                },
                "|",
                {
                    name: "table",
                    action: EasyMDE.drawTable,
                    className: "fa fa-table",
                    title: "Insert Table",
                },
                {
                    name: "horizontal-rule",
                    action: EasyMDE.drawHorizontalRule,
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
                    name: "print",
                    action: S.editor.print,
                    className: "fa fa-print",
                    title: "Print Page",
                },
                {
                    name: "preview",
                    action: S.editor.preview,
                    className: "fa fa-eye no-disable",
                    title: "Toggle Preview",
                },
                "|",
                {
                    name: "side-by-side",
                    action: S.editor.sidebyside,
                    className: "fa fa-columns no-disable no-mobile",
                    title: "Toggle Side by Side",
                },
                {
                    name: "fullscreen",
                    action: S.editor.fullscreen,
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

        //set up print function
        $(window).on('afterprint', function () {
            var toggle = S.editor.toggled;
            if (toggle.preview) { S.editor.preview(...toggle.arguments); }
            S.editor.toggled = null;
        });
    },

    resize: function () {
        var win = S.window.pos();
        var pos = S.editor.div.offset(); 
        S.editor.div.css({ height: win.h - pos.top - win.scrolly });
    },

    getContent: function (entryId) {
        if (S.editor.entryId == entryId) { return; }
        S.editor.updated.stop(); //stop auto-save timer
        if (editor.value() != '' && S.editor.changed == true) { S.editor.save(); }
        S.editor.setContent('');
        S.editor.entryId = entryId;
        S.ajax.post('Entries/LoadEntry', { entryId: entryId, bookId: S.entries.bookId },
            function (d) {
                S.editor.setContent(d);
                $('.entries .entry.selected').removeClass('selected');
                $('.entries .entry.entryid-' + entryId).addClass('selected');
                var win = S.window.pos();
                if ($('.subbar.show-card').length > 0 && win.w <= 895) {
                    //mobile view, show page
                    S.menus.page.show();
                }
            },
            function (err) {
                S.message.show('.popup .message', 'error', err);
            }
        );
    },

    setContent: function (content) {
        S.editor.updated.stop(); //stop auto-save timer
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

    insertText: function (text) {
        var cm = editor.codemirror;
        var doc = cm.getDoc();
        var cursor = doc.getCursor();
        var line = doc.getLine(cursor.line);
        var pos = {
            line: cursor.line
        };
        if (line.length === 0) {
            // if line is empty, add text
            doc.replaceRange(text, pos);
        } else {
            // add a new line, then text
            doc.replaceRange("\n" + text, pos);
        }
    },

    changed: false,

    updated: {
        timer: null,

        stop: function () {
            clearTimeout(S.editor.updated.timer);
        },

        check: function () {
            S.editor.changed = true;
            clearTimeout(S.editor.updated.timer);
            S.editor.updated.timer = setTimeout(function () { S.editor.save(); }, 5000); //auto-save after 5 seconds of no activity
        }
    },

    save: function () {
        S.editor.changed = false;
        var data = {
            entryId: S.editor.entryId,
            content: editor.value()
        };
        S.ajax.post('Entries/SaveEntry', data,
            function (d) {
            },
            function (err) {
                S.message.show('.popup .message', 'error', err);
            }
        );
    },

    guide: {
        show: function () {

        },
        hide: function () {

        }
    },

    info: {
        show: function (callback) {
            var data = { bookId:S.entries.bookId, entryId:S.editor.entryId };
            S.ajax.post('Entries/LoadEntryInfo', data,
                function (d) {
                    S.popup.show('Entry Details', d, { width: 350 });
                    $('.popup form').on('submit', S.editor.info.submit);
                    if (typeof callback == 'function') {
                        callback();
                    }
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );
            
        },

        newChapter: function () {
            S.editor.info.temp = {
                title: $('#txtentry_title').val(),
                summary: $('#txtentry_summary').val()
            };
            S.chapters.create.view(S.editor.info.createdChapter);
        },

        createdChapter: function (chapter) {
            var temp = S.editor.info.temp;
            S.editor.info.show(() => {
                $('#txtentry_title').val(temp.title);
                $('#txtentry_summary').val(temp.summary);
            });
        },

        submit: function (e) {
            e.preventDefault();
            e.cancelBubble = true;
            let date = $('#txtentry_datecreated').val();
            if (date == '' || (new Date(date)).toString() == 'Invalid Date') {
                S.message.show('.popup .message', 'error', 'Please provide a valid date & time');
                return false;
            }
            var data = {
                entryId: S.editor.entryId,
                bookId: parseInt($('#lstentry_book').val()),
                datecreated: date,
                title: $('#txtentry_title').val(),
                summary: $('#txtentry_summary').val(),
                chapter: parseInt($('#lstentry_chapter').val()),
            };

            if (data.title == '') {
                S.message.show('.popup .message', 'error', 'Please provide a title for your entry');
                return false;
            }
            if (data.summary == '') {
                S.message.show('.popup .message', 'error', 'Please provide a summary for your entry');
                return false;
            }

            S.ajax.post('Entries/UpdateEntryInfo', data,
                function (d) {
                    S.popup.hide();
                    //view book based on selected book within entry info form
                    S.entries.view(data.bookId, true);
                },
                function (err) {
                    S.message.show('.popup .message', 'error', err);
                }
            );

            return false;
        }
    },

    sidebyside: function () {
        S.editor.toggle.sidebyside = !S.editor.toggle.sidebyside;
        EasyMDE.toggleSideBySide(...arguments);
    },

    preview: function () {
        S.editor.toggle.preview = !S.editor.toggle.preview;
        S.editor.toggle.print = false;
        EasyMDE.togglePreview(...arguments);
    },

    fullscreen: function () {
        S.editor.toggle.fullscreen = !S.editor.toggle.fullscreen;
        S.editor.toggle.print = false;
        EasyMDE.toggleFullScreen(...arguments);
    },

    print: function () {
        var toggle = { preview: false, fullscreen: false };
        if (S.editor.toggle.preview == false) {
            S.editor.preview(...arguments);
            toggle.preview = true;
        }
        S.editor.toggle.print = true;
        toggle.arguments = arguments;
        S.editor.toggled = toggle;
        setTimeout(window.print, 100);
    },

    uploader: {
        upload: function (input) {
            if (input.files && input.files.length > 0) {
                //get current caret position in editor

                var files = input.files;
                var progress = $('.upload-progress');
                progress.prop({ 'width': '1%' });
                progress.show();
                for (var x = 0; x < files.length; x++) {
                    var xhr = new XMLHttpRequest();
                    var file = files[x];
                    //show progress bar
                    xhr.upload.addEventListener('progress', (e) => {
                        var percent = (x / files.length * 100) + ((e.loaded / e.total * 100) / files.length);
                        $('.upload-progress').prop({ 'width': percent + '%' });
                    }, false);

                    xhr.open('POST', '/upload?entryId=' + S.editor.entryId, false);

                    xhr.onload = function () {
                        if (xhr.status >= 200 && xhr.status < 400) {
                            //request success
                            console.log(xhr.responseText);
                            let uploads = JSON.parse(xhr.responseText);
                            console.log(uploads);
                            if (uploads != null && uploads.length > 0) {
                                for (var y = 0; y < uploads.length; y++) {
                                    var f = uploads[y];
                                    switch (f.Type) {
                                        case 1: //image
                                            S.editor.insertText('![' + f.Name + '](' + f.Path + f.Name + ')');
                                            break;
                                        default: //link to file
                                            S.editor.insertText('[' + f.Name + '](' + f.Path + f.Name + ')');
                                            break;
                                    }
                                    
                                }
                            }
                            
                        }
                    };

                    console.log('sending file...');
                    var formData = new FormData();
                    formData.append("file", file);
                    xhr.send(formData);
                }
                progress.hide();
            }
        }
    },

    image: {
        showDialog: function () {
            uploader.click();
        },

        render: function (md, opt) {
            md.inline.ruler.push("image-link", (state, checkMode) => {
                var src = state.src;
                if (src.indexOf('![') == 0) {
                    if (!checkMode) {
                        //parse image
                        var images = src.split('\n');
                        for (var x = 0; x < images.length; x++) {
                            var parts = images[x].replace('![', '').replace('](', '|').replace(')', '').split('|');
                            console.log('"' + images[x] + '"');
                            state.push({
                                content: '<a href="' + parts[1] + '/full" target="_blank"><img src="' + parts[1] + '" alt="' + parts[0] + '"/></a>',
                                type: 'htmltag',
                                level: state.level
                            });
                        }
                    }
                    state.pos += state.src.length;
                    return true;
                }
                return false;
            });
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
    init: function() {
        S.trash.resize();
        S.scrollbar.add($('.trash .container'), { touch: true });
    },

    resize: function () {
        //resize trash height
        const win = S.window.pos();
        const container = $('.subbar .trash .container');
        const pos = container.position();
        container.css({ height: win.h - pos.top - 1 });
    },

    view: function () {
        S.dash.hideAll();
        S.ajax.post('Trash/LoadTrash', {},
            function (d) {
                $('.sidebar > .menu .item-trash').addClass('selected');
                $('.trash').html(d);
                $('.trash, .trash-details').removeClass('hide');
                S.trash.init();
            }
        );
    },

    select: function () {
        if ($('.trash .checkbox.checked').length > 0) {
            $('.trash-details .selected-items').removeClass('hide');
        } else {
            $('.trash-details .selected-items').addClass('hide');
        }
    }
};

/* Menus */
S.menus = {
    books: {
        show: function () {
            $('.sidebar').removeClass('hide-card').addClass('show-card');
            var hide = $('.subbar');
            if (hide.hasClass('show-card')) {
                hide.addClass('hide-card').removeClass('show-card');
                setTimeout(() => { hide.removeClass('hide-card'); }, 1000);
            }
        }
    },

    chapters: {
        show: function () {
            $('.subbar').removeClass('hide-card').addClass('show-card');
            var hide = $('.sidebar');
            if (hide.hasClass('show-card')) {
                hide.addClass('hide-card').removeClass('show-card');
                setTimeout(() => { hide.removeClass('hide-card'); }, 1000);
            }
        }
    },

    page: {
        show: function () {
            var hide = $('.sidebar');
            if (hide.hasClass('show-card')) {
                hide.addClass('hide-card').removeClass('show-card');
                setTimeout(() => { hide.removeClass('hide-card'); }, 1000);
            }
            var hide2 = $('.subbar');
            if (hide2.hasClass('show-card')) {
                hide2.addClass('hide-card').removeClass('show-card');
                setTimeout(() => { hide2.removeClass('hide-card'); }, 1000);
            }
        }
    }
};
