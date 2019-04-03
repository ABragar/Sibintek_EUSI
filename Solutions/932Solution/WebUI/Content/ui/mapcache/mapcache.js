var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var Event = (function () {
            function Event() {
                this._isDisposed = false;
                this._handlers = [];
            }
            Event.prototype.notify = function (sender, event) {
                if (sender == null) {
                    throw new Common.ArgumentNullException("sender");
                }
                if (event == null) {
                    throw new Common.ArgumentNullException("event");
                }
                if (this.handlers.length === 1) {
                    var handler = this.handlers[0];
                    handler(sender, event);
                }
                else if (this.handlers.length > 0) {
                    for (var _i = 0, _a = this.handlers; _i < _a.length; _i++) {
                        var handler = _a[_i];
                        handler(sender, event);
                    }
                }
            };
            Object.defineProperty(Event.prototype, "empty", {
                get: function () {
                    return this.handlers.length === 0;
                },
                enumerable: true,
                configurable: true
            });
            Event.prototype.add = function (handler) {
                if (typeof handler !== "function") {
                    throw new Common.ArgumentNullException("handler");
                }
                this._handlers.push(handler);
                this.doAdd(handler);
            };
            Event.prototype.doAdd = function (handler) {
            };
            Event.prototype.remove = function (handler) {
                if (typeof handler !== "function") {
                    throw new Common.ArgumentNullException("handler");
                }
                if (this._handlers.length > 0) {
                    var removeIndex = this._handlers.indexOf(handler);
                    if (removeIndex !== -1) {
                        var removeHandler = this._handlers[removeIndex];
                        this._handlers.splice(removeIndex, 1);
                        this.doRemove(removeHandler);
                    }
                }
            };
            Event.prototype.doRemove = function (handler) {
            };
            Object.defineProperty(Event.prototype, "handlers", {
                get: function () {
                    return this._handlers;
                },
                enumerable: true,
                configurable: true
            });
            Event.prototype.clear = function () {
                this._handlers = [];
            };
            return Event;
        }());
        Common.Event = Event;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="../common/event.ts" />
var Lib;
(function (Lib) {
    var Console;
    (function (Console) {
        var TerminalExEvent = (function (_super) {
            __extends(TerminalExEvent, _super);
            function TerminalExEvent() {
                _super.apply(this, arguments);
            }
            return TerminalExEvent;
        }(Lib.Common.Event));
        Console.TerminalExEvent = TerminalExEvent;
        ;
        var TerminalEx = (function () {
            function TerminalEx(containerNode, options) {
                this._isRunning = false;
                this._inited = new TerminalExEvent();
                this._ctrl = new TerminalExEvent();
                this._command = new TerminalExEvent();
                this._exited = new TerminalExEvent();
                if (containerNode == null) {
                    throw new Lib.Common.ArgumentNullException("containerNode");
                }
                var terminalId = ++TerminalEx._nodeNextId;
                this._terminalNode = this.createTerminalNode(terminalId);
                containerNode.appendChild(this._terminalNode);
                var termOptions = this.createTerminalOptions(terminalId, this._terminalNode.id, options);
                this._terminal = new Terminal(termOptions);
                TermGlobals.webColors = [];
            }
            TerminalEx.prototype.createTerminalOptions = function (terminalId, nodeId, options) {
                var termOptions = {
                    bgColor: TerminalEx._defaultBgColor
                };
                if (typeof options === "object") {
                    Object.keys(options).map(function (k) { return termOptions[k] = options[k]; });
                }
                termOptions.termDiv = nodeId;
                termOptions.id = terminalId;
                termOptions.handler = this.handleCommand.bind(this);
                termOptions.initHandler = this.handleInit.bind(this);
                termOptions.ctrlHandler = this.handleCtrl.bind(this);
                termOptions.exitHandler = this.handleExit.bind(this);
                return termOptions;
            };
            TerminalEx.prototype.createTerminalNode = function (terminalId) {
                var node = document.createElement("div");
                node.className = "term-ex";
                node.id = "term-ex-" + terminalId;
                return node;
            };
            Object.defineProperty(TerminalEx.prototype, "terminalNode", {
                get: function () {
                    return this._terminalNode;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "inited", {
                get: function () {
                    return this._inited;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "ctrl", {
                get: function () {
                    return this._ctrl;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "command", {
                get: function () {
                    return this._command;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "exited", {
                get: function () {
                    return this._exited;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "term", {
                get: function () {
                    return this._terminal;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "running", {
                get: function () {
                    return this._isRunning;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "isReady", {
                get: function () {
                    return this._isRunning && !this._terminal.closed;
                },
                enumerable: true,
                configurable: true
            });
            TerminalEx.prototype.open = function () {
                if (!this._isRunning || this._terminal.closed) {
                    this._terminal.open();
                    this._isRunning = true;
                }
                else {
                    this._terminal.focus();
                }
            };
            TerminalEx.prototype.close = function (destroy) {
                if (!this._terminal.closed) {
                    this._terminal.close();
                }
                if (destroy) {
                    this.destroy();
                }
            };
            TerminalEx.prototype.error = function (text) {
                if (this.isReady) {
                    this._terminal.write("%c(red)" + text + "%c0");
                }
            };
            TerminalEx.prototype.prompt = function () {
                this._terminal.prompt();
            };
            TerminalEx.prototype.write = function (text, usemore) {
                this._terminal.write(text, usemore);
            };
            TerminalEx.prototype.newLine = function () {
                this._terminal.newLine();
            };
            Object.defineProperty(TerminalEx.prototype, "argv", {
                get: function () {
                    return this._terminal.argv;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TerminalEx.prototype, "argc", {
                get: function () {
                    return this._terminal.argc;
                },
                set: function (value) {
                    this._terminal.argc = value;
                },
                enumerable: true,
                configurable: true
            });
            TerminalEx.prototype.handleInit = function () {
                this._terminal.write(this._terminal.conf.greeting);
                this._terminal.newLine();
                this._terminal.prompt();
                this.RaiseEvent(this._inited);
            };
            TerminalEx.prototype.handleExit = function () {
                this.RaiseEvent(this._exited);
            };
            TerminalEx.prototype.handleCtrl = function () {
                this.RaiseEvent(this._ctrl);
            };
            TerminalEx.prototype.handleCommand = function () {
                this.RaiseEvent(this._command);
            };
            TerminalEx.prototype.RaiseEvent = function (event) {
                if (!event.empty) {
                    event.notify(this, Lib.Common.EventArgs.empty);
                }
            };
            TerminalEx.prototype.destroyTerminalNode = function () {
                if (this._terminalNode) {
                    if (this._terminalNode.parentNode) {
                        this._terminalNode.parentNode.removeChild(this._terminalNode);
                    }
                    this._terminalNode = null;
                }
            };
            TerminalEx.prototype.destroyTerminal = function () {
                if (this._terminal) {
                    this.close();
                    this._terminal.handler = null;
                    this._terminal.ctrlHandler = null;
                    this._terminal.initHandler = null;
                    this._terminal.exitHandler = null;
                    this._terminal = null;
                }
            };
            TerminalEx.prototype.destroyEvents = function () {
                this._inited.clear();
                this._command.clear();
                this._ctrl.clear();
                this._exited.clear();
                this._inited = null;
                this._command = null;
                this._ctrl = null;
                this._exited = null;
            };
            TerminalEx.prototype.destroy = function () {
                this.destroyTerminal();
                this.destroyTerminalNode();
                this.destroyEvents();
            };
            TerminalEx._defaultBgColor = "#232e45";
            TerminalEx._nodeNextId = 0;
            return TerminalEx;
        }());
        Console.TerminalEx = TerminalEx;
    })(Console = Lib.Console || (Lib.Console = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Console;
    (function (Console) {
        var CommandProcessor = (function () {
            function CommandProcessor(terminal) {
                this._running = false;
                this._terminal = terminal;
                this._commands = new Lib.Common.List();
                this._parser = new Parser();
                this.handleInitTerminal = this.handleInitTerminal.bind(this);
                this.handleCommandTerminal = this.handleCommandTerminal.bind(this);
                this.handleCtrlTerminal = this.handleCtrlTerminal.bind(this);
                this.handleExitTerminal = this.handleExitTerminal.bind(this);
            }
            Object.defineProperty(CommandProcessor.prototype, "terminal", {
                get: function () {
                    return this._terminal;
                },
                enumerable: true,
                configurable: true
            });
            CommandProcessor.prototype.run = function () {
                if (!this._running) {
                    this.registerTerminalEvents();
                }
                this._terminal.open();
                this.helpPage();
                this._running = true;
            };
            CommandProcessor.prototype.exit = function () {
                this._terminal.close();
                this.unregisterTerminalEvents();
                this._running = false;
            };
            CommandProcessor.prototype.helpPage = function () {
                if (this._terminal.isReady) {
                    this.term.clear();
                    this.term.write(this.generateHelpText());
                    this.term.newLine();
                    this.term.prompt();
                }
            };
            CommandProcessor.prototype.generateHelpText = function () {
                var helpText = [
                    ("%+r **** " + CommandProcessor._version + " **** %-r"),
                    " ",
                    "* use \"help\" to see this page.",
                    "* use \"[command] help\" to see specific [command] help page.",
                    "* use \"clear\" to clear the terminal.",
                    "* use \"exit\" to quit.",
                    " ",
                    "** list of commands: **"
                ];
                var commandText = this.generateCommandText();
                if (commandText.length > 0) {
                    helpText.push(" ");
                    helpText = helpText.concat(commandText);
                }
                else {
                    helpText.push("no.");
                }
                helpText.push(" ");
                return helpText;
            };
            CommandProcessor.prototype.generateCommandText = function () {
                var textLines = [];
                this._commands.toArray().forEach(function (command) {
                    textLines.push("* \"" + command.name + "\" " + command.description);
                });
                return textLines;
            };
            CommandProcessor.prototype.addCommand = function (command) {
                this._commands.add(command);
            };
            CommandProcessor.prototype.removeCommand = function (command) {
                this._commands.remove(command);
            };
            CommandProcessor.prototype.findCommand = function (name) {
                var result = null;
                for (var i = 0, s = this._commands.size; i < s; i++) {
                    var command = this._commands.get(i);
                    if (command.name.toLowerCase() === name.toLowerCase()) {
                        result = command;
                    }
                }
                return result;
            };
            //#region Handle Terminal Events
            CommandProcessor.prototype.registerTerminalEvents = function () {
                this._terminal.inited.add(this.handleInitTerminal);
                this._terminal.command.add(this.handleCommandTerminal);
                this._terminal.ctrl.add(this.handleCtrlTerminal);
                this._terminal.exited.add(this.handleExitTerminal);
            };
            CommandProcessor.prototype.unregisterTerminalEvents = function () {
                this._terminal.inited.remove(this.handleInitTerminal);
                this._terminal.command.remove(this.handleCommandTerminal);
                this._terminal.ctrl.remove(this.handleCtrlTerminal);
                this._terminal.exited.remove(this.handleExitTerminal);
            };
            CommandProcessor.prototype.handleInitTerminal = function () {
                // Nothing
            };
            Object.defineProperty(CommandProcessor.prototype, "term", {
                get: function () {
                    return this._terminal.term;
                },
                enumerable: true,
                configurable: true
            });
            CommandProcessor.prototype.handleCommandTerminal = function () {
                this.term.newLine();
                this._parser.parseLine(this.term);
                if (this.term.argv.length === 0) {
                    this.term.prompt();
                    return;
                }
                if (this.term.argQL[0]) {
                    this._terminal.error("Syntax error: first argument quoted.");
                    this.term.prompt();
                    return;
                }
                var cmd = this.term.argv[this.term.argc++];
                switch (cmd) {
                    case "help":
                        this.helpPage();
                        return;
                    case "clear":
                        this.term.clear();
                        this.term.prompt();
                        return;
                    case "exit":
                        this.exit();
                        return;
                    default:
                        var command = this.findCommand(cmd);
                        if (command != null) {
                            var helpCmd = this.term.argv[this.term.argc];
                            if (helpCmd === "help" || helpCmd === "-?") {
                                this.term.write(command.help());
                                this.term.prompt();
                            }
                            else {
                                command.excecute(this._terminal, this._parser);
                            }
                        }
                        else {
                            this.term.write("Unknown command: " + cmd + ". See \"help\" for available commands.");
                            this.term.prompt();
                        }
                        return;
                }
            };
            CommandProcessor.prototype.handleCtrlTerminal = function () {
                // Nothing
            };
            CommandProcessor.prototype.handleExitTerminal = function () {
                // Nothing
            };
            //#endregion Handle Terminal Events
            CommandProcessor.prototype.destroy = function () {
                this.unregisterTerminalEvents();
                this._commands.clear();
                this._commands = null;
                this._terminal = null;
                this._parser = null;
            };
            CommandProcessor._version = "cmd-0.0.1";
            return CommandProcessor;
        }());
        Console.CommandProcessor = CommandProcessor;
    })(Console = Lib.Console || (Lib.Console = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Console;
    (function (Console) {
        var CommandBase = (function () {
            function CommandBase() {
                this._waitTimer = null;
                this._waitChars = ["\\", "|", "/", "-"];
                this._waitInterval = 50;
                this.name = "unknown";
                this.description = "unknown";
            }
            CommandBase.prototype.error = function (term, text, prompt) {
                if (prompt === void 0) { prompt = true; }
                term.error(text);
                if (prompt) {
                    term.prompt();
                }
            };
            CommandBase.prototype.success = function (term, text, prompt) {
                if (prompt === void 0) { prompt = true; }
                term.write("%c(green)" + text + "%c0");
                if (prompt) {
                    term.prompt();
                }
            };
            CommandBase.prototype.warning = function (term, text, prompt) {
                if (prompt === void 0) { prompt = true; }
                term.write(text);
                if (prompt) {
                    term.prompt();
                }
            };
            CommandBase.prototype.wait = function (term) {
                var _this = this;
                this.clearWait(term);
                var charIndex = 0;
                this._waitTimer = window.setInterval(function () {
                    if (charIndex >= _this._waitChars.length) {
                        charIndex = 0;
                    }
                    term.term.backspace();
                    term.write(_this._waitChars[charIndex++]);
                }, this._waitInterval);
            };
            CommandBase.prototype.clearWait = function (term) {
                if (this._waitTimer != null) {
                    term.term.backspace();
                    window.clearInterval(this._waitTimer);
                    this._waitTimer = null;
                }
            };
            return CommandBase;
        }());
        Console.CommandBase = CommandBase;
    })(Console = Lib.Console || (Lib.Console = {}));
})(Lib || (Lib = {}));
/// <reference path="lib/console/commandbase.ts" />
var MapCache;
(function (MapCache) {
    var ManageCacheCommand = (function (_super) {
        __extends(ManageCacheCommand, _super);
        function ManageCacheCommand(mapCacheService, baseGroupKey) {
            _super.call(this);
            this.name = "cache";
            this.description = "Manage map cache. Please use \"cache help\" for more details.";
            this._serivce = mapCacheService;
            this._baseGroupKey = baseGroupKey;
        }
        ManageCacheCommand.prototype.excecute = function (term, parser) {
            var opts = this.parseOpt(term, parser);
            var cmd = term.argv[term.argc++];
            switch (cmd) {
                case "list":
                    this.handleListCommand(term);
                    break;
                case "info":
                    this.handleInfoCommand(term);
                    break;
                case "update":
                    this.handleUpdateCommand(term);
                    break;
                case "clear":
                    this.handleClearCommand(term);
                    break;
                case "stats":
                    this.handleStatsCommand(term, opts);
                    break;
                case "on":
                    this.handleEnableCommand(term);
                    break;
                case "off":
                    this.handleDisableCommand(term);
                    break;
                default:
                    term.write(this.help());
                    term.prompt();
                    break;
            }
        };
        ManageCacheCommand.prototype.parseAndValidArgs = function (term) {
            var cmd = term.argv[term.argc++];
            var all = cmd === "all";
            var index = cmd != null && /^\d+$/.test(cmd.trim()) ? +cmd : 0;
            if (!all && index <= 0) {
                this.error(term, "Expected keyword \"all\" or numeric index.");
                return null;
            }
            return { all: all, index: index };
        };
        ManageCacheCommand.prototype.parseOpt = function (term, parser) {
            var resetOpt = parser.getopt(term.term, "r");
            return {
                resetOpt: resetOpt["r"] !== undefined
            };
        };
        ManageCacheCommand.prototype.handleListCommand = function (term) {
            this.findAllGroupsAsync(term, "list", function (data) {
                data.forEach(function (item, i) {
                    term.write((i + 1) + ". " + item.Title + " (" + item.Key + ")");
                    term.newLine();
                });
                term.newLine();
                term.prompt();
            });
        };
        ManageCacheCommand.prototype.handleUpdateCommand = function (term) {
            var _this = this;
            var args = this.parseAndValidArgs(term);
            if (args == null) {
                return;
            }
            if (args.index > 0) {
                this.findGroupByIndexAsync(term, "update", args.index, function (data) {
                    _this.updateCache(term, data, 0, function () { return false; });
                });
            }
            else if (args.all) {
                this.findAllGroupsAsync(term, "update", function (data) {
                    var next = function (current) {
                        var nextIndex = current + 1;
                        if (nextIndex >= data.length) {
                            return false;
                        }
                        _this.updateCache(term, data[nextIndex], nextIndex, next);
                        return true;
                    };
                    _this.updateCache(term, data[0], 0, next);
                });
            }
        };
        ManageCacheCommand.prototype.updateCache = function (term, group, index, next) {
            var _this = this;
            this.wait(term);
            this._serivce.updateCacheAsync(group.Key).then(function (res) {
                _this.clearWait(term);
                term.write((index + 1) + ". " + group.Title + " ");
                _this.success(term, "successfully updated " + res.count + " items.", false);
                term.newLine();
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            }).catch(function (e) {
                _this.clearWait(term);
                term.error((index + 1) + ". " + e);
                term.newLine();
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            });
        };
        ManageCacheCommand.prototype.handleClearCommand = function (term) {
            var _this = this;
            var args = this.parseAndValidArgs(term);
            if (args == null) {
                return;
            }
            if (args.index > 0) {
                this.findGroupByIndexAsync(term, "clear", args.index, function (data) {
                    _this.wait(term);
                    _this._serivce.clearCacheAsync(data.Key).then(function (res) {
                        _this.clearWait(term);
                        if (res) {
                            _this.success(term, "Cache was successfully cleared.");
                        }
                        else {
                            _this.error(term, "Oops! unknown result.");
                        }
                    }).catch(function (e) {
                        _this.clearWait(term);
                        _this.error(term, e);
                    });
                });
            }
            else if (args.all) {
                this.wait(term);
                this._serivce.clearAllCacheAsync().then(function (res) {
                    _this.clearWait(term);
                    if (res) {
                        _this.success(term, "Сache was successfully cleared.");
                    }
                    else {
                        _this.error(term, "Oops! unknown result.");
                    }
                }).catch(function (e) {
                    _this.clearWait(term);
                    _this.error(term, e);
                });
            }
        };
        ManageCacheCommand.prototype.handleInfoCommand = function (term) {
            var _this = this;
            var args = this.parseAndValidArgs(term);
            if (args == null) {
                return;
            }
            if (args.index > 0) {
                this.findGroupByIndexAsync(term, "info", args.index, function (data) {
                    _this.wait(term);
                    _this._serivce.getCacheInfoAsync(data.Key).then(function (res) {
                        _this.clearWait(term);
                        if (typeof res !== "object") {
                            _this.error(term, "Oops! unknown result.");
                            return;
                        }
                        _this.printInfo(term, res);
                        term.newLine();
                        term.prompt();
                    }).catch(function (e) {
                        _this.clearWait(term);
                        _this.error(term, e);
                    });
                });
            }
            else if (args.all) {
                this.wait(term);
                this._serivce.getAllCacheInfoAsync().then(function (res) {
                    _this.clearWait(term);
                    if (res == null || typeof res !== "object" || res.length === undefined) {
                        _this.error(term, "Oops! unknown result.");
                        return;
                    }
                    if (res.length === 0) {
                        term.write("List of cache info is empty.");
                        term.prompt();
                        return;
                    }
                    res.forEach(function (info) {
                        _this.printInfo(term, info);
                        term.newLine();
                    });
                    term.prompt();
                }).catch(function (e) {
                    _this.clearWait(term);
                    _this.error(term, e);
                });
            }
        };
        ManageCacheCommand.prototype.handleStatsCommand = function (term, opts) {
            var _this = this;
            if (opts.resetOpt) {
                this.wait(term);
                this._serivce.resetCacheStatsAsync().then(function (res) {
                    _this.clearWait(term);
                    if (res) {
                        _this.success(term, "Cache statistics was successfully reseted.");
                    }
                    else {
                        _this.error(term, "Oops! unknown result.");
                    }
                }).catch(function (e) {
                    _this.clearWait(term);
                    _this.error(term, e);
                });
            }
            else {
                this.wait(term);
                this._serivce.getCacheStatsAsync().then(function (res) {
                    _this.clearWait(term);
                    if (typeof res !== "object") {
                        _this.error(term, "Oops! unknown result.");
                        return;
                    }
                    _this.printStats(term, res);
                    term.newLine();
                    term.prompt();
                }).catch(function (e) {
                    _this.clearWait(term);
                    _this.error(term, e);
                });
            }
        };
        ManageCacheCommand.prototype.handleEnableCommand = function (term) {
            var _this = this;
            var args = this.parseAndValidArgs(term);
            if (args == null) {
                return;
            }
            if (args.index > 0) {
                this.findGroupByIndexAsync(term, "on", args.index, function (data) {
                    _this.enableCache(term, data, 0, function () { return false; });
                });
            }
            else if (args.all) {
                this.findAllGroupsAsync(term, "on", function (data) {
                    var next = function (current) {
                        var nextIndex = current + 1;
                        if (nextIndex >= data.length) {
                            return false;
                        }
                        _this.enableCache(term, data[nextIndex], nextIndex, next);
                        return true;
                    };
                    _this.enableCache(term, data[0], 0, next);
                });
            }
        };
        ManageCacheCommand.prototype.enableCache = function (term, group, index, next) {
            var _this = this;
            this.wait(term);
            this._serivce.enableCacheAsync(group.Key).then(function (res) {
                _this.clearWait(term);
                if (res) {
                    term.write((index + 1) + ". " + group.Title + " ");
                    _this.success(term, "enabled.", false);
                    term.newLine();
                }
                else {
                    _this.error(term, (index + 1) + ". Oops! unknown result.", false);
                    term.newLine();
                }
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            }).catch(function (e) {
                _this.clearWait(term);
                term.error((index + 1) + ". " + e);
                term.newLine();
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            });
        };
        ManageCacheCommand.prototype.handleDisableCommand = function (term) {
            var _this = this;
            var args = this.parseAndValidArgs(term);
            if (args == null) {
                return;
            }
            if (args.index > 0) {
                this.findGroupByIndexAsync(term, "off", args.index, function (data) {
                    _this.disableCache(term, data, 0, function () { return false; });
                });
            }
            else if (args.all) {
                this.findAllGroupsAsync(term, "off", function (data) {
                    var next = function (current) {
                        var nextIndex = current + 1;
                        if (nextIndex >= data.length) {
                            return false;
                        }
                        _this.disableCache(term, data[nextIndex], nextIndex, next);
                        return true;
                    };
                    _this.disableCache(term, data[0], 0, next);
                });
            }
        };
        ManageCacheCommand.prototype.disableCache = function (term, group, index, next) {
            var _this = this;
            this.wait(term);
            this._serivce.disableCacheAsync(group.Key).then(function (res) {
                _this.clearWait(term);
                if (res) {
                    term.write((index + 1) + ". " + group.Title + " ");
                    _this.success(term, "disabled.", false);
                    term.newLine();
                }
                else {
                    _this.error(term, (index + 1) + ". Oops! unknown result.", false);
                    term.newLine();
                }
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            }).catch(function (e) {
                _this.clearWait(term);
                term.error((index + 1) + ". " + e);
                term.newLine();
                if (!next(index)) {
                    term.newLine();
                    term.prompt();
                }
            });
        };
        ManageCacheCommand.prototype.printInfo = function (term, info) {
            term.write("** \u0413\u0440\u0443\u043F\u043F\u0430: " + info.Group.Title);
            term.newLine();
            term.write("   \u041A\u043E\u043B-\u0432\u043E \u0437\u0430\u043F\u0438\u0441\u0435\u0439 \u0432 \u043A\u044D\u0448\u0435: " + info.CachedItems);
            term.newLine();
            term.write("   \u0420\u0430\u0437\u043C\u0435\u0440 \u0438\u0441\u043F\u043E\u043B\u044C\u0437\u0443\u0435\u043C\u043E\u0439 \u043F\u0430\u043C\u044F\u0442\u0438: " + (info.UsedMemorySize === -1 ? "неопределено" : info.UsedMemorySize + " \u0431\u0430\u0439\u0442"));
            term.newLine();
        };
        ManageCacheCommand.prototype.printStats = function (term, stats) {
            term.write("** \u041F\u0440\u043E\u0446\u0435\u043D\u0442 \u043F\u043E\u043F\u0430\u0434\u0430\u043D\u0438\u0439 \u0432 \u043A\u044D\u0448: " + stats.HitRate + "%");
            term.newLine();
            term.write("   \u041F\u0440\u043E\u0446\u0435\u043D\u0442 \u043F\u0440\u043E\u043C\u0430\u0445\u043E\u0432: " + stats.MissRate + "%");
            term.newLine();
            term.write("   \u041A\u043E\u043B-\u0432\u043E \u043F\u043E\u043F\u0430\u0434\u0430\u043D\u0438\u0439 \u0432 \u043A\u044D\u0448: " + stats.Hits);
            term.newLine();
            term.write("   \u041A\u043E\u043B-\u0432\u043E \u043F\u0440\u043E\u043C\u0430\u0445\u043E\u0432: " + stats.Miss);
            term.newLine();
            term.write("   \u041A\u043E\u043B-\u0432\u043E \u0437\u0430\u043F\u0440\u043E\u0441\u043E\u0432: " + stats.Requests);
            term.newLine();
        };
        ManageCacheCommand.prototype.findAllGroupsAsync = function (term, opName, onComplete) {
            var _this = this;
            this.wait(term);
            this._serivce.getCacheGroupsAsync(this._baseGroupKey).then(function (data) {
                _this.clearWait(term);
                if (data.length <= 0) {
                    _this.warning(term, "Nothing to " + opName + ". List of group is empty.");
                    return;
                }
                if (onComplete != null) {
                    onComplete(data);
                }
            }).catch(function (error) {
                _this.clearWait(term);
                _this.error(term, error);
            });
        };
        ManageCacheCommand.prototype.findGroupByIndexAsync = function (term, opName, groupIndex, onComplete) {
            var _this = this;
            this.findAllGroupsAsync(term, opName, function (data) {
                if (groupIndex > 0 && data[groupIndex - 1] != null) {
                    if (onComplete != null) {
                        onComplete(data[groupIndex - 1]);
                    }
                }
                else {
                    _this.error(term, "Could not found group with index: " + groupIndex);
                }
            });
        };
        ManageCacheCommand.prototype.help = function () {
            return [
                "  Manage cache:",
                "     cache list ............. list of cache groups",
                "     cache info all ......... get cache info for all groups",
                "     cache info <key> ....... get cache info for specific group",
                "     cache stats ............ get cache statistics",
                "     cache -r stats ......... reset cache statistics",
                "     cache update all ....... update cache for all groups",
                "     cache update <key> ..... update cache for specific group",
                "     cache clear all ........ clear cache for all groups",
                "     cache clear <key> ...... clear cache for specific group",
                "     cache on all ........... enable cache for all groups",
                "     cache on <key> ......... enable cache for specific group",
                "     cache off all .......... disable cache for all groups",
                "     cache off <key> ........ disable cache for specific group",
                "     cache -? ............... show this help page",
                "     cache help ............. show this help page"
            ];
        };
        return ManageCacheCommand;
    }(Lib.Console.CommandBase));
    MapCache.ManageCacheCommand = ManageCacheCommand;
})(MapCache || (MapCache = {}));
/// <reference path="lib/console/terminalex.ts" />
/// <reference path="lib/console/commandprocessor.ts" />
/// <reference path="managecachecommand.ts" />
var MapCache;
(function (MapCache) {
    var ConsoleProgram = (function () {
        function ConsoleProgram() {
        }
        ConsoleProgram.run = function (containerNode, endPoint, baseGroupKey) {
            if (ConsoleProgram._containerNode != null && ConsoleProgram._containerNode !== containerNode) {
                ConsoleProgram.destroy();
            }
            if (ConsoleProgram._commandProcessor) {
                ConsoleProgram._commandProcessor.run();
                return;
            }
            ConsoleProgram._terminal = new MapCache.TerminalExWrapper(containerNode, { cols: 125 });
            ConsoleProgram._commandProcessor = new Lib.Console.CommandProcessor(ConsoleProgram._terminal);
            var mapCacheService = new MapCache.ManageCacheService(endPoint);
            ConsoleProgram._commandProcessor.addCommand(new MapCache.ManageCacheCommand(mapCacheService, baseGroupKey));
            ConsoleProgram._commandProcessor.run();
            ConsoleProgram._containerNode = containerNode;
        };
        ConsoleProgram.exit = function () {
            if (ConsoleProgram._commandProcessor) {
                ConsoleProgram._commandProcessor.exit();
            }
        };
        ConsoleProgram.destroy = function () {
            if (ConsoleProgram._commandProcessor) {
                ConsoleProgram._commandProcessor.destroy();
                ConsoleProgram._commandProcessor = null;
            }
            if (ConsoleProgram._terminal) {
                ConsoleProgram._terminal.destroy();
                ConsoleProgram._terminal = null;
            }
            ConsoleProgram._containerNode = null;
        };
        ConsoleProgram._commandProcessor = null;
        ConsoleProgram._terminal = null;
        ConsoleProgram._containerNode = null;
        return ConsoleProgram;
    }());
    MapCache.ConsoleProgram = ConsoleProgram;
})(MapCache || (MapCache = {}));
var MapCache;
(function (MapCache) {
    var ManageCacheService = (function () {
        function ManageCacheService(endPoint) {
            this._endPoint = endPoint;
        }
        ManageCacheService.prototype.getCacheInfoAsync = function (groupKey) {
            return this.requestGet("GetCacheInfo", { groupKey: groupKey });
        };
        ManageCacheService.prototype.getAllCacheInfoAsync = function () {
            return this.requestGet("GetAllCacheInfo");
        };
        ManageCacheService.prototype.getCacheGroupsAsync = function (baseGroupKey) {
            return this.requestGet("GetCacheGroups", (baseGroupKey != null ? { baseGroupKey: baseGroupKey } : undefined));
        };
        ManageCacheService.prototype.getCacheStatsAsync = function () {
            return this.requestGet("GetCacheStats");
        };
        ManageCacheService.prototype.resetCacheStatsAsync = function () {
            return this.requestPost("ResetCacheStats");
        };
        ManageCacheService.prototype.enableCacheAsync = function (groupKey) {
            return this.requestPost("EnableCache", { groupKey: groupKey });
        };
        ManageCacheService.prototype.disableCacheAsync = function (groupKey) {
            return this.requestPost("DisableCache", { groupKey: groupKey });
        };
        ManageCacheService.prototype.updateCacheAsync = function (groupKey) {
            return this.requestPost("UpdateCache", { groupKey: groupKey });
        };
        ManageCacheService.prototype.clearCacheAsync = function (groupKey) {
            return this.requestPost("ClearCache", { groupKey: groupKey });
        };
        ManageCacheService.prototype.clearAllCacheAsync = function () {
            return this.requestPost("ClearAllCache");
        };
        ManageCacheService.prototype.requestGet = function (servicePath, data) {
            return this.sendRequest("GET", servicePath, data);
        };
        ManageCacheService.prototype.requestPost = function (servicePath, data) {
            return this.sendRequest("POST", servicePath, data);
        };
        ManageCacheService.prototype.sendRequest = function (method, servicePath, data) {
            var _this = this;
            return new Promise(function (resolve, reject) {
                jQuery.ajax({
                    type: method,
                    url: _this.makeUrl(servicePath),
                    data: data,
                    dataType: "json",
                    cache: false
                }).done(function (response) {
                    if (response && response.error) {
                        reject(response.error);
                    }
                    else {
                        resolve(response);
                    }
                }).fail(function (xhr) {
                    reject("Request error: " + xhr.status + " " + xhr.statusText);
                });
            });
        };
        ManageCacheService.prototype.makeUrl = function (servicePath) {
            return this._endPoint + "/" + servicePath;
        };
        return ManageCacheService;
    }());
    MapCache.ManageCacheService = ManageCacheService;
})(MapCache || (MapCache = {}));
/// <reference path="lib/console/terminalex.ts" />
var MapCache;
(function (MapCache) {
    var TerminalExWrapper = (function (_super) {
        __extends(TerminalExWrapper, _super);
        function TerminalExWrapper(containerNode, options) {
            _super.call(this, containerNode, options);
            this._wrapperNode = this.createWrapperNode(containerNode);
            this.wrapTerminal(this._wrapperNode, containerNode);
        }
        TerminalExWrapper.prototype.createWrapperNode = function (containerNode) {
            var node = document.createElement("div");
            node.className = "term-ex-wrap";
            return node;
        };
        TerminalExWrapper.prototype.wrapTerminal = function (wrapperNode, containerNode) {
            if (this.terminalNode.parentNode != null) {
                this.terminalNode.parentNode.removeChild(this.terminalNode);
            }
            wrapperNode.appendChild(this.terminalNode);
            containerNode.appendChild(wrapperNode);
        };
        TerminalExWrapper.prototype.open = function () {
            this._wrapperNode.style.display = "block";
            _super.prototype.open.call(this);
        };
        TerminalExWrapper.prototype.close = function (destroy) {
            this._wrapperNode.style.display = "none";
            _super.prototype.close.call(this, destroy);
        };
        TerminalExWrapper.prototype.destroy = function () {
            _super.prototype.destroy.call(this);
            if (this._wrapperNode.parentNode) {
                this._wrapperNode.parentNode.removeChild(this._wrapperNode);
            }
            this._wrapperNode = null;
        };
        return TerminalExWrapper;
    }(Lib.Console.TerminalEx));
    MapCache.TerminalExWrapper = TerminalExWrapper;
})(MapCache || (MapCache = {}));
/// <reference path="../typings/tsd.d.ts" /> 
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var Exception = (function (_super) {
            __extends(Exception, _super);
            function Exception(message, innerException) {
                _super.call(this);
                if (!(this instanceof Exception)) {
                    return new Exception(message, innerException);
                }
                if (typeof Error.captureStackTrace === "function") {
                    Error.captureStackTrace(this, arguments.callee);
                }
                this.name = "Exception";
                message = message == null ? "" : message;
                if (innerException != null) {
                    if (innerException instanceof Error) {
                        this.innerException = innerException;
                        this.message = message + ", innerException: " + this.innerException.message;
                    }
                    else if (typeof innerException === "string") {
                        this.innerException = new Exception(innerException);
                        this.message = message + ", innerException: " + this.innerException.message;
                    }
                    else {
                        this.innerException = new Exception(innerException.toString());
                        this.message = message + ", innerException: " + this.innerException.message;
                    }
                }
                else {
                    this.message = message;
                }
            }
            Exception.prototype.toString = function () {
                var resultString;
                if (this.message == null ||
                    (typeof this.message === "string" && this.message.length <= 0)) {
                    resultString = this.name;
                }
                else {
                    resultString = this.name + ": " + this.message;
                }
                if (this.innerException != null) {
                    resultString += " ---> " + this.innerException.toString();
                }
                if (this.stack != null) {
                    resultString += "\n" + this.stack;
                }
                return resultString;
            };
            return Exception;
        }(Error));
        Common.Exception = Exception;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var ArgumentException = (function (_super) {
            __extends(ArgumentException, _super);
            function ArgumentException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentException";
            }
            return ArgumentException;
        }(Common.Exception));
        Common.ArgumentException = ArgumentException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var ArgumentNullException = (function (_super) {
            __extends(ArgumentNullException, _super);
            function ArgumentNullException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentNullException";
            }
            return ArgumentNullException;
        }(Common.Exception));
        Common.ArgumentNullException = ArgumentNullException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var ArgumentOutOfRangeException = (function (_super) {
            __extends(ArgumentOutOfRangeException, _super);
            function ArgumentOutOfRangeException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ArgumentOutOfRangeException";
            }
            return ArgumentOutOfRangeException;
        }(Common.Exception));
        Common.ArgumentOutOfRangeException = ArgumentOutOfRangeException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var AssertException = (function (_super) {
            __extends(AssertException, _super);
            function AssertException(message) {
                _super.call(this, message);
                this.name = "AssertException";
            }
            return AssertException;
        }(Common.Exception));
        Common.AssertException = AssertException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var Debug = (function () {
            function Debug() {
            }
            Debug.assert = function (condition, message) {
                if (!condition) {
                    message = message || "Assertion failed";
                    throw new Common.AssertException(message);
                }
            };
            return Debug;
        }());
        Common.Debug = Debug;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var EventArgs = (function () {
            function EventArgs() {
            }
            Object.defineProperty(EventArgs, "empty", {
                get: function () {
                    return new EventArgs();
                },
                enumerable: true,
                configurable: true
            });
            return EventArgs;
        }());
        Common.EventArgs = EventArgs;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var InvalidOperationException = (function (_super) {
            __extends(InvalidOperationException, _super);
            function InvalidOperationException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "InvalidOperationException";
            }
            return InvalidOperationException;
        }(Common.Exception));
        Common.InvalidOperationException = InvalidOperationException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var List = (function () {
            function List() {
                this._items = [];
            }
            Object.defineProperty(List.prototype, "size", {
                get: function () {
                    return this._items.length;
                },
                enumerable: true,
                configurable: true
            });
            List.prototype.add = function (item) {
                this._items.push(item);
            };
            List.prototype.remove = function (item) {
                var index = this._items.indexOf(item);
                if (index >= 0) {
                    this._items.splice(index, 1);
                }
            };
            List.prototype.contains = function (item) {
                return this._items.indexOf(item) >= 0;
            };
            List.prototype.indexOf = function (item) {
                return this._items.indexOf(item);
            };
            List.prototype.get = function (index) {
                if (index >= this._items.length) {
                    throw new Common.ArgumentOutOfRangeException("index");
                }
                return this._items[index];
            };
            List.prototype.set = function (index, item) {
                if (index >= this._items.length) {
                    throw new Common.ArgumentOutOfRangeException("index");
                }
                this._items[index] = item;
            };
            List.prototype.clear = function () {
                this._items = [];
            };
            List.prototype.toArray = function () {
                return this._items.slice();
            };
            return List;
        }());
        Common.List = List;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var NotImplementedException = (function (_super) {
            __extends(NotImplementedException, _super);
            function NotImplementedException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "NotImplementedException";
            }
            return NotImplementedException;
        }(Common.Exception));
        Common.NotImplementedException = NotImplementedException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
/// <reference path="exception.ts" />
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var ObjectDisposedException = (function (_super) {
            __extends(ObjectDisposedException, _super);
            function ObjectDisposedException(message, innerException) {
                _super.call(this, message, innerException);
                this.name = "ObjectDisposedException";
            }
            return ObjectDisposedException;
        }(Common.Exception));
        Common.ObjectDisposedException = ObjectDisposedException;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
var Lib;
(function (Lib) {
    var Common;
    (function (Common) {
        var ElapsedEventArgs = (function (_super) {
            __extends(ElapsedEventArgs, _super);
            function ElapsedEventArgs() {
                _super.apply(this, arguments);
                this.payload = null;
            }
            return ElapsedEventArgs;
        }(Common.EventArgs));
        Common.ElapsedEventArgs = ElapsedEventArgs;
        var Timer = (function () {
            function Timer(timeout) {
                // Events
                this._elapsed = new Common.Event();
                Common.Debug.assert(timeout > 0);
                this._timeout = timeout;
                this._running = false;
                this._windowTimers = window;
                this._timerDescriptor = -1;
                this.handleTimeout = this.handleTimeout.bind(this);
            }
            Object.defineProperty(Timer.prototype, "elapsed", {
                get: function () {
                    return this._elapsed;
                },
                enumerable: true,
                configurable: true
            });
            Timer.prototype.start = function (payload) {
                var _this = this;
                if (!this._running) {
                    this._timerDescriptor = this._windowTimers.setTimeout(function () {
                        _this._running = false;
                        _this._timerDescriptor = -1;
                        _this.handleTimeout(payload);
                    }, this._timeout);
                }
            };
            Timer.prototype.stop = function () {
                if (this._timerDescriptor > 0) {
                    this._windowTimers.clearTimeout(this._timerDescriptor);
                    this._timerDescriptor = -1;
                }
                this._running = false;
            };
            Timer.prototype.handleTimeout = function (payload) {
                if (!this._elapsed.empty) {
                    var eventArgs = new ElapsedEventArgs();
                    eventArgs.payload = payload;
                    this._elapsed.notify(this, eventArgs);
                }
            };
            Timer.prototype.destroy = function () {
                this.stop();
                this._elapsed.clear();
                this._elapsed = null;
                this._windowTimers = null;
            };
            return Timer;
        }());
        Common.Timer = Timer;
    })(Common = Lib.Common || (Lib.Common = {}));
})(Lib || (Lib = {}));
