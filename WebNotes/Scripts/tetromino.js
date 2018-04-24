'use strict';
let States = {
  I: [
    [[0, 0], [1, 0], [2, 0], [3, 0]],
    [[0, 0], [0, 1], [0, 2], [0, 3]]
  ],
  J: [
    [[0, 0], [1, 0], [2, 0], [2, 1]],
    [[1, 0], [1, 1], [1, 2], [0, 2]],
    [[0, 0], [0, 1], [1, 1], [2, 1]],
    [[1, 0], [0, 0], [0, 1], [0, 2]]
  ],
  L: [
    [[0, 1], [0, 0], [1, 0], [2, 0]],
    [[0, 0], [1, 0], [1, 1], [1, 2]],
    [[0, 1], [1, 1], [2, 1], [2, 0]],
    [[0, 0], [0, 1], [0, 2], [1, 2]]
  ],
  O: [
    [[0, 0], [1, 0], [0, 1], [1, 1]]
  ],
  S: [
    [[0, 1], [1, 1], [1, 0], [2, 0]],
    [[0, 0], [0, 1], [1, 1], [1, 2]]
  ],
  T: [
    [[0, 0], [1, 0], [2, 0], [1, 1]],
    [[1, 0], [1, 1], [1, 2], [0, 1]],
    [[0, 1], [1, 1], [2, 1], [1, 0]],
    [[0, 0], [0, 1], [0, 2], [1, 1]]
  ],
  Z: [
    [[0, 0], [1, 0], [1, 1], [2, 1]],
    [[1, 0], [1, 1], [0, 1], [0, 2]]
  ]
}

const DEFAULT_SALT = "Lorem Ipsum";
const SALT_INPUT_POINTER = '.form-horizontal input[type=email]';
const TABLE_DESK_CLASS = 'table.desk';
const WORK_AREA = '.g_auth';
const TABLE_POINTER = WORK_AREA + ' ' + TABLE_DESK_CLASS;
const SIGIL_IMAGE_CLASS = '.sigil';
const SIGIL_POINTER = WORK_AREA + ' ' + SIGIL_IMAGE_CLASS;
const SIGIL_POSITION_ATTRIBUTE = 'position';
const SIGIL_ROTATION_ATTRIBUTE = 'retation';
const SIGIL_STATE_ATTRIBUTE = 'state';
const PLACED_CLASS = '.placed';
const PLACED = 'placed';
const SIGIL_ID_PREFIX = '#sigil_';
const OUTPUT_POINTER = 'textarea#print_hash';
const TRASH_POINTER = WORK_AREA + ' ' + '.trash';

class DeskData {
  constructor(sizeX, sizeY) {
    this.sheet = Array(sizeY);
    for(let pos = 0; pos < this.sheet.length; pos++) {
      this.sheet[pos] = Array(sizeX).fill(null);
    }
  }
  insert(position, state) {
    if(this.isFree(position.cell_x, position.cell_y, state.offset)) {
      state.offset.forEach(offset => {
        let cell_x = position.cell_x + offset[0];
        let cell_y = position.cell_y + offset[1];
        this.sheet[cell_y][cell_x] = state.name;
      });
    }
  }
  remove(position, state) {
    if(this.validate(position.cell_x, position.cell_y, state)) {
      state.offset.forEach(offset => {
        let cell_x = position.cell_x + offset[0];
        let cell_y = position.cell_y + offset[1];
        this.sheet[cell_y][cell_x] = null;
      });
    }
  }
  clear() {
    for(let row in this.sheet) {
      for(let cell in this.sheet[row]){
        this.sheet[row][cell] = null;
      }
    }
  }
  isFree(x, y, state) {
    let result = true;
    state.forEach(offset => {
      let cell_x = x + offset[0];
      let cell_y = y + offset[1];
      result = result && this.sheet[cell_y][cell_x] == null;
    });
    return result;
  }
  validate(x, y, state) {
    let result = true;
    state.offset.forEach(offset => {
      let cell_x = x + offset[0];
      let cell_y = y + offset[1];
      result = result && this.sheet[cell_y][cell_x] == state.name;
    });
    return result;
  }
  encrypt(salt) {
    salt = salt == undefined ? DEFAULT_SALT : salt;
    let data = sha3_224(salt);
    this.sheet.forEach(row => {
      row.forEach(cell => {
        data += cell;
      });
    });
    return keccak256(data);
  }
  print() {
    this.sheet.forEach(row => {
      console.log(row);
    });
  }
}

function rotate(jobject) {
  let sigil = $(jobject);
    let rotation = sigil.attr(SIGIL_ROTATION_ATTRIBUTE);
    let state = sigil.attr(SIGIL_STATE_ATTRIBUTE);
    if(rotation) {
      rotation = ++rotation % States[state].length;
    }
    else {
      rotation = States[state].length > 1 ? 1 : 0;
    }
    let translate = '';
    if((rotation % 2) != 0) {
      let vtr_x = BlockImages[state].vtr_x;
      let vtr_y = BlockImages[state].vtr_y;
      if(rotation == 3) {
        vtr_x = -vtr_x;
        vtr_y = -vtr_y;
      }
      translate = `translate(${vtr_x}%, ${vtr_y}%)`;
    }
    let degrees = rotation * 90;
    sigil.css({
      '-webkit-transform' : 'rotate('+ degrees +'deg) ' + translate,
      '-moz-transform' : 'rotate('+ degrees +'deg) ' + translate,
      '-ms-transform' : 'rotate('+ degrees +'deg) ' + translate,
      'transform' : 'rotate('+ degrees +'deg) ' + translate
    });
    sigil.attr(SIGIL_ROTATION_ATTRIBUTE, rotation);
}

const Types = Object.keys(States);

const Desk = {
  width: 0,
  height: 0,
  cell: {
    width: 0,
    height: 0,
    count_x: 0,
    count_y: 0
  },
  round: (x, y, block, vertical, offset) => {
    x -= offset.left;
    y -= offset.top;
    if(x < 0) x = 0;
    if(y < 0) y = 0;
    let cell_offset_x = (Desk.width / Desk.cell.count_x);
    let cell_offset_y = (Desk.height / Desk.cell.count_y);
    let cell_x = Math.round(x / cell_offset_x);
    let cell_y = Math.round(y / cell_offset_y);
    x = cell_x * Desk.cell.width + offset.left;
    y = cell_y * Desk.cell.height + offset.top;
    return { 
      x: x,
      y: y,
      cell_x: cell_x,
      cell_y: cell_y
    };
  }
}

const BlockImages = {
  I: { width: 4, height: 1, vtr_x: '38', vtr_y: '150'},
  O: { width: 2, height: 2, vtr_x: '0', vtr_y: '0'},
  S: { width: 3, height: 2, vtr_x: '17', vtr_y: '24'},
  Z: { width: 3, height: 2, vtr_x: '17', vtr_y: '24'},
  L: { width: 3, height: 2, vtr_x: '17', vtr_y: '24'},
  J: { width: 3, height: 2, vtr_x: '17', vtr_y: '24'},
  T: { width: 3, height: 2, vtr_x: '17', vtr_y: '24'},
}

// JQuery UI dependencies code

$(() => {
    let desk = $(TABLE_POINTER);
    Desk.width = desk.width();
    Desk.height = desk.height();
    Desk.cell.count_x = desk.children('tbody').children('tr:first').children().length;
    Desk.cell.count_y = desk.children('tbody').children().length;
    Desk.cell.width = Desk.width / Desk.cell.count_x;
    Desk.cell.height = Desk.height / Desk.cell.count_y;
    for (let block in BlockImages) {
        $(SIGIL_ID_PREFIX + block).css({
            width: Desk.cell.width * BlockImages[block].width,
            height: Desk.cell.height * BlockImages[block].height
        });
    }
    let data = new DeskData(Desk.cell.count_x, Desk.cell.count_y);

    let get_salt = () => {
        return $(SALT_INPUT_POINTER).val();
    }

    $(SIGIL_POINTER).draggable({
        helper: 'clone',
        cursor: 'move',
        scope: 'sigil',
        start: (event, ui) => {
            ui.helper.css({ "z-index": 1001 });
        }
    }).click((event) => {
        rotate(event.target);
    });

    $(TABLE_POINTER).droppable({
        scope: 'sigil',
        tolerance: 'fit',
        drop: (event, ui) => {
            let sigil = ui.draggable.hasClass(PLACED) ? ui.draggable : ui.draggable.clone();
            let rotation = sigil.attr(SIGIL_ROTATION_ATTRIBUTE);
            let position = Desk.round(
                ui.offset.left,
                ui.offset.top,
                BlockImages[sigil.attr(SIGIL_STATE_ATTRIBUTE)],
                (rotation % 2) != 0,
                $(TABLE_POINTER).offset()
            );
            if (!rotation) rotation = 0;
            data.insert(position, {
                name: sigil.attr(SIGIL_STATE_ATTRIBUTE),
                offset: States[sigil.attr(SIGIL_STATE_ATTRIBUTE)][rotation]
            });
            $(OUTPUT_POINTER).val(data.encrypt(get_salt()));
            sigil.css({
                top: position.y,
                left: position.x,
                position: 'absolute',
                "z-index": 1000
            }).addClass(PLACED)
                .appendTo(TABLE_POINTER)
                .data(SIGIL_POSITION_ATTRIBUTE, position)
                .draggable({
                    cursor: 'move',
                    scope: 'sigil',
                    start: (event, ui) => {
                        ui.helper.css({ "z-index": 1001 });
                        let rotation = ui.helper.attr(SIGIL_ROTATION_ATTRIBUTE);
                        if (!rotation) rotation = 0;
                        data.remove(ui.helper.data(SIGIL_POSITION_ATTRIBUTE), {
                            name: ui.helper.attr(SIGIL_STATE_ATTRIBUTE),
                            offset: States[ui.helper.attr(SIGIL_STATE_ATTRIBUTE)][rotation]
                        });
                        $(OUTPUT_POINTER).val(data.encrypt(get_salt()));
                    }
                });
        }
    });
    $(TRASH_POINTER).droppable({
        scope: 'sigil',
        drop: (event, ui) => {
            if (ui.draggable.hasClass(PLACED)) {
                let rotation = ui.draggable.attr(SIGIL_ROTATION_ATTRIBUTE);
                if (!rotation) rotation = 0;
                data.remove(ui.draggable.data(SIGIL_POSITION_ATTRIBUTE), {
                    name: ui.draggable.attr(SIGIL_STATE_ATTRIBUTE),
                    offset: States[ui.draggable.attr(SIGIL_STATE_ATTRIBUTE)][rotation]
                });
                $(OUTPUT_POINTER).val(data.encrypt(get_salt()));
                ui.draggable.remove();
            }
        }
    }).click((event) => {
        $(PLACED_CLASS).remove();
        data.clear();
        $(OUTPUT_POINTER).val(data.encrypt(get_salt()));
    });
    $("input[type=submit]").click((event) => {
        $(OUTPUT_POINTER).val(data.encrypt(get_salt()));
    });
});