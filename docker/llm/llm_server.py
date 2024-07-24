from flask import Flask, request, jsonify
import subprocess

app = Flask(__name__)

@app.route('/generate', methods=['POST'])
def generate():
    prompt = request.json['prompt']

    result = subprocess.run(['./main', '-m', './models/llama-2-7b.ggmlv3.q4_0.bin', '-p', prompt],
                            capture_output=True, text=True)

    return jsonify({'generated_text': result.stdout})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)