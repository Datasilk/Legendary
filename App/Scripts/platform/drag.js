S.drag = {
    items: [], timer: null, timerStart: null,

    item: {
        index: null,
        elem: null,
        win: { w: 0, h: 0, scrollx: 0, scrolly: 0 },
        pos: { x: 0, y: 0, w: 0, h: 0 },
        start: { x: 0, y: 0 },
        cursor: { x: 0, y: 0 },
        options: null,
        hasOnDrag: false
    },

    add: function (elem, dragElem, onStart, onDrag, onStop, onClick, options) {
        //options = { boundTop:0 , boundRight:0 , boundLeft:0 , boundRight:0, useElemPos:false, callee:S.drag, hideArea: false, hideAreaOffset: 0 }
        this.items.push({ elem: elem, dragElem: dragElem, onStart: onStart, onDrag: onDrag, onStop: onStop, onClick: onClick, options: options });
        var x = this.items.length - 1;
        $(elem).on('mousedown', function (e) { S.drag.events.start.call(S.drag, x, e) });
    },

    has: function (elem) {
        this.items.forEach(function (item) {
            if (item.elem == elem) { return true; }
        });
        return false;
    },

    alteredDOM: function () {
        //manually call this function if the DOM was altered and 
        //changed the position of the drag element's parent element
        S.drag.item.parent.y = S.drag.item.element.parent().offset().top;
    },

    events: {
        start: function (index, e) {
            var self = this;
            var item = this.items[index];
            var delay = 200;
            if (item.options) {
                if (item.options.delay != null) {
                    delay = item.options.delay;
                }
            }
            this.item = { index: index };
            this.timer = null;
            this.timerStart = setTimeout(function () {
                //allow a delay for user click before starting drag animation
                var el = $(item.elem);
                var elem = $(item.dragElem);
                var elpos = el.offset();
                var pos = elem.position();
                var parentPos = elem.parent().offset();
                var win = $(window);
                var speed = 1000 / 30;
                var hideArea = false;
                var hideAreaOffset = 0;
                if (item.options) {
                    if (item.options.useElemPos == true) {
                        pos = { left: elpos.left - pos.left, top: elpos.top - pos.top };
                    }
                    if (item.options.speed != null) {
                        speed = item.options.speed;
                    }
                    hideArea = item.options.hideArea || false;
                    hideAreaOffset = item.options.hideAreaOffset || 0;
                    if (hideArea == true) {
                        //hide the relative area that the DOM element being dragged previously occupied
                        //by subtracting the element's height from the element's margin, which will
                        //cover the element's tracks & force siblings below (in the DOM) to be pushed upwards
                        elem.css({ marginBottom: -1 * (elem.height() + hideAreaOffset) });
                    }
                }

                self.item = {
                    index: index,
                    elem: item.dragElem,
                    element: elem,
                    win: {
                        w: win.width(),
                        h: win.height()
                    },
                    start: {
                        x: e.clientX + document.body.scrollLeft,
                        y: e.clientY + document.body.scrollTop
                    },
                    cursor: {
                        x: e.clientX + document.body.scrollLeft,
                        y: e.clientY + document.body.scrollTop
                    },
                    pos: {
                        x: pos.left,
                        y: pos.top,
                        w: elem.width(),
                        h: elem.height()
                    },
                    parent: {
                        y: parentPos.top,
                        y1: parentPos.top
                    },
                    options: item.options,
                    hasOnDrag: typeof item.onDrag == 'function',
                    hideArea: hideArea,
                    hideAreaOffset: hideAreaOffset
                }

                if (typeof item.onStart == 'function') {
                    item.onStart.call(item.options ? (item.options.callee ? item.options.callee : this) : this, item);
                }

                //set up document-level drag events
                $(document).on('mousemove', S.drag.events.doc.move);
                
                S.drag.events.drag.call(S.drag);
                clearInterval(this.timer);
                self.timer = setInterval(function () { S.drag.events.drag.call(S.drag); }, speed);
            }, delay);

            //set up document-level click event
            $(document).on('mouseup', S.drag.events.doc.up);

            //don't let drag event select text on the page
            if (e.stopPropagation) e.stopPropagation();
            if (e.preventDefault) e.preventDefault();
            e.cancelBubble = true;
            e.returnValue = false;
            return false;
        },

        doc: {
            move: function (e) { S.drag.events.mouse.call(S.drag, e); },
            up: function (e) { S.drag.events.stop.call(S.drag) }
        },

        mouse: function (e) {
            this.item.cursor.x = e.clientX + document.body.scrollLeft;
            this.item.cursor.y = e.clientY + document.body.scrollTop;
            this.item.parent.y = this.item.element.parent().offset().top;
        },

        drag: function () {
            var item = this.item;
            if (item.hasOnDrag == true) { if (this.items[item.index].onDrag.call(item.options ? (item.options.callee ? item.options.callee : this) : this, item) == false) { return; } }
            var x = (item.pos.x + (item.cursor.x - item.start.x));
            var y = (item.pos.y + (item.cursor.y - item.start.y) - (item.parent.y - item.parent.y1));
            if (item.options) {
                if (item.options.boundTop != null) {
                    if (item.options.boundTop > y) { y = item.options.boundTop; }
                }
                if (item.options.boundRight != null) {
                    if (item.win.w - item.options.boundRight < x + item.pos.w) { x = item.win.w - item.options.boundRight - item.pos.w; }
                }
                if (item.options.boundBottom != null) {
                    if (item.win.h - item.options.boundBottom < y + item.pos.h) { y = item.win.h - item.options.boundBottom - item.pos.h; }
                }
                if (item.options.boundLeft != null) {
                    if (item.options.boundLeft > x) { x = item.options.boundLeft; }
                }
            }
            item.elem[0].style.left = x + 'px';
            item.elem[0].style.top = y + 'px';
        },

        stop: function (index) {
            var item = S.drag.items[S.drag.item.index];
            $(document).off('mouseup', S.drag.events.doc.up);
            if (this.timer == null) {
                //user click
                clearTimeout(this.timerStart);
                this.timerStart = null;
                if (typeof item.onClick == 'function') {
                    item.onClick.call(item.options ? (item.options.callee ? item.options.callee : this) : this, item);
                }
                return;
            }
            clearInterval(this.timer);
            this.timer == null;
            $(document).off('mousemove', S.drag.events.doc.move);
            if (S.drag.item.hideArea == true) {
                S.drag.item.elem.css({ marginBottom: 0 });
            }
            if (typeof item.onStop == 'function') {
                item.onStop.call(item.options ? (item.options.callee ? item.options.callee : this) : this, item);
            }
        }
    }
};