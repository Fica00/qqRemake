const express = require("express");
const path = require("path");
const TelegramBot = require("node-telegram-bot-api");
const TOKEN = "7409919859:AAEyGTHrOqZBnqPVciOW4L4RObgMe2w-U8E";
const server = express();
const bot = new TelegramBot(TOKEN, {
    polling: true
});
const port = process.env.PORT || 5000;

server.use(express.static(path.join(__dirname, 'test-qoomon')));

const sendGameMessage = (msg) => {
    const chatId = msg.chat.id;
    const imageUrl = 'https://pbs.twimg.com/media/GPiGKvdagAAvq5Y.jpg';

    const caption = `*Hello! Welcome to Qoomon Quest*\nQoomon Quest is a fast-paced crypto card game that lets you win and earn. Launch the 3min game here.\nWe will definitely appreciate your efforts once the token is listed (details to be announced).\nDon’t forget to invite your friends!`;
    bot.sendPhoto(chatId, imageUrl, {
        caption: caption,
        parse_mode: "Markdown",
        reply_markup: {
            inline_keyboard: [
                [
                    {
                        text: "Play Now",
                        web_app: {
                            url: "https://qqtelegram.s3.amazonaws.com/index.html?a=Telegram"
                        }
                    }
                ]
            ]
        }
    }).catch(err => console.error(err));
};

bot.onText(/start/, (msg) => sendGameMessage(msg));
bot.onText(/game/, (msg) => sendGameMessage(msg));

bot.onText(/help/, (msg) => bot.sendMessage(msg.from.id, "Say /game or /start if you want to play."));

server.listen(port);
