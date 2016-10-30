EXENAME = reference
CXX = clang++
CXXFLAGS = -std=c++0x -g -O0 -Wall -Wextra

all : $(EXENAME)

$(EXENAME): reference.cpp
	$(CXX) $(CXXFLAGS) reference.cpp -o $(EXENAME)

.PHONY: clean
clean:
	rm -f $(EXENAME)