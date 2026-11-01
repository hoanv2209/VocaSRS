// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function speakWord(word) {
    console.log(word);

    if (!("speechSynthesis" in window)) {
        console.warn("Trình duyệt không hỗ trợ SpeechSynthesis");
        return;
    }

    // Dừng câu đọc trước nếu người dùng bấm liên tục
    speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(word);
    utterance.lang = "en-US";
    utterance.rate = 0.85;

    const voices = speechSynthesis.getVoices();

    const englishVoice = voices.find(v =>
        v.lang.toLowerCase() === "en-us"
    );

    if (englishVoice) {
        utterance.voice = englishVoice;
    }

    speechSynthesis.speak(utterance);
}
