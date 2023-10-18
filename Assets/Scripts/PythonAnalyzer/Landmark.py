"""

/**
 * This is part of the Gaze Tracker package v2.4
 * @author C. N. Spencer
 */

Landmark class for SMG study
Landmark objects have a name, and two lists: one for the durations of looks and one for the duration in between looks
"""


class Landmark:
    name = ""
    looks = []
    offs = []   # starts after first glance at object (i.e. first exit=1 in csv data)

    def __init__(self, name="Default Object", looks=None, offs=None):
        self.name = name
        if looks is not None:
            self. looks = looks
        if offs is not None:
            self.offs = offs

    def __str__(self):
        return f"{self.name}: {len(self.looks)} looks, {len(self.offs)} offs"
