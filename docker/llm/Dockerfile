FROM ubuntu:22.04

RUN apt-get update && apt-get install -y \
    git \
    build-essential \
    cmake \
    python3 \
    python3-pip \
    wget

RUN git clone https://github.com/ggerganov/llama.cpp.git
WORKDIR /llama.cpp
RUN make

# Create models directory if it doesn't exist
RUN mkdir -p models

# Download a smaller Llama 2 model (you'll need to provide the actual download link)
RUN wget -O models/llama-2-7b.ggmlv3.q4_0.bin https://huggingface.co/TheBloke/Llama-2-7B-GGML/resolve/main/llama-2-7b.ggmlv3.q4_0.bin

# Install Flask for the API
RUN pip3 install flask

COPY llm_server.py .

EXPOSE 5000

CMD ["python3", "llm_server.py"]