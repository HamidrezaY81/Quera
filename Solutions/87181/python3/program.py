import re
from collections import Counter


def words_check(text):
    # Split the text into words based on spaces
    words = text.split()

    processed_words = []
    for word in words:
        # Remove non-alphabet characters from the word
        cleaned_word = re.sub(r'[^a-zA-Z]', '', word)

        # Skip words with more than half non-alphabet characters
        if len(cleaned_word) <= (len(word) / 2):
            continue

        # Standardize format: first letter uppercase, others lowercase
        standardized_word = cleaned_word.capitalize()
        processed_words.append(standardized_word)

    # Count the occurrences of each word and return as dictionary
    word_counts = Counter(processed_words)
    return dict(word_counts)
